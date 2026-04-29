using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI_Base : MonoBehaviour
{
    #region General Variables
    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent; //Ref al cerebro NavMesh del objeto
    [SerializeField] Transform target; //Ref a la posición del target a perseguir
    [SerializeField] LayerMask targetLayer; //Define la capa del target (Detección)
    [SerializeField] LayerMask groundLayer; //Define la capa del suelo (Definir puntos navegables)

    [Header("Patroling Stats")]
    [SerializeField] float walkPointRange = 8f; //Radio máximo de margen espacial para buscar puntos navegables
    Vector3 walkPoint; //Posición del punto a perseguir
    bool walkPointSet; //Si es falso, busca punto. Si es verdadero, no puede buscar punto

    // NUEVO: Variables para el sistema modular de Waypoints
    [Header("Waypoint Patrol System")]
    [SerializeField] bool useWaypoints; // Checkbox para decidir qué modo de patrulla usar
    [SerializeField] Transform[] waypoints; // Array para arrastrar los transforms del escenario
    private int currentWaypointIndex; // Índice interno para saber a qué waypoint toca ir

    [Header("Attacking Stats")]
    [SerializeField] float timeBetweenAttacks = 1f; //Tiempo entre ataque y ataque
    bool alreadyAttacked; //Se pregunta si estamos atacando para no stackear ataques

    [Header("Melee Attack")]
    [SerializeField] float damage = 10f;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius = 1.5f;

    [Header("States & Detection Areas")]
    [SerializeField] float sightRange = 8f; //Radio de la detección de persecución
    [SerializeField] float attackRange = 2f; //Radio de la detección del ataque
    [SerializeField] bool targetInSightRange; //Determina si entra el estado PERSEGUIR
    [SerializeField] bool targetInAttackRange; //Determina si entra el estado ATACAR

    [Header("Stuck Detection")]
    [SerializeField] float stuckCheckTime = 2f; //Tiempo que el agente espera quieto antes de preguntarse si está stuck
    [SerializeField] float stuckThreshold = 0.1f; //Margen de detección de stuck
    [SerializeField] float maxStuckDuration = 3f; //Tiempo máximo de estar stuck

    float stuckTimer; //Reloj que cuenta el tiempo de estar stuck
    float lastCheckTime; //Tiempo de chequeo previo a estar stuck
    Vector3 lastPosition; //Posición del último walkpoint perseguido
    #endregion


    Animator anim; //Ref al animator

    private void Awake()
    {
        //Validación por si no encontramos al "Player" por nombre, para evitar NullReferenceExceptions
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) target = playerObj.transform;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        lastPosition = transform.position;
        lastCheckTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyStateUpdater();
        CheckIfStuck();
    }

    void EnemyStateUpdater()
    {
        //Acción que se encarga de la gestión de los estados de la IA
        //Esfera de detección física
        Collider[] hits = Physics.OverlapSphere(transform.position, sightRange, targetLayer);
        targetInSightRange = hits.Length > 0;

        //Si está persiguiendo, calcula la distancia hasta que el mínimo entre dentro del rango de ataque
        if (targetInSightRange)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            targetInAttackRange = distance <= attackRange;
        }
        else
        {
            //Si el player sale del sightRange, forzamos que el ataque sea falso por seguridad
            targetInAttackRange = false;
        }

        //Lógica de los cambios de estado
        if (!targetInSightRange && !targetInAttackRange) Patroling();
        else if (targetInSightRange && !targetInAttackRange) ChaseTarget();
        else if (targetInSightRange && targetInAttackRange) AttackTarget();
    }

    void Patroling()
{
    if (useWaypoints && waypoints.Length > 0)
    {
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
    else
    {
        // Patrulla random (tu sistema original)
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        if (!agent.pathPending && agent.remainingDistance <= 0.5f && walkPointSet)
        {
            walkPointSet = false;
        }
    }
}

    void SearchWalkPoint()
    {
        //Acción que busca un punto de patrulla random si no lo hay
        int attempts = 0; //Número interno de intentos de buscar punto nuevo
        const int maxAttempts = 5;

        while (!walkPointSet && attempts < maxAttempts)
        {
            attempts++;
            Vector3 randomPoint = transform.position + new Vector3(Random.Range(-walkPointRange, walkPointRange), 0, Random.Range(-walkPointRange, walkPointRange));

            // Chequear si el punto está en un lugar en el que haya NavMesh Surface
            // Con SamplePosition es suficiente para saber que el punto existe en el NavMesh.
            // Eliminamos el Raycast físico para evitar dependencias de LayerMasks mal configuradas en el Inspector.
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                walkPoint = hit.position; //Determina el Vector3 random a perseguir
                walkPointSet = true; //Tenemos punto y el agente va hacia él
            }
        }

        

    }

    void ChaseTarget()
    {
        //Le dice al agente que persiga al target
        agent.SetDestination(target.position);
        anim.SetBool("isChasing", targetInSightRange);
    }

    void AttackTarget()
    {
    // Se queda quieto
    agent.SetDestination(transform.position);
       

        // Mira al jugador
        Vector3 direction = (target.position - transform.position);
    direction.y = 0;

    if (direction != Vector3.zero)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            lookRotation,
            agent.angularSpeed * Time.deltaTime
        );
    }

    // Ataque con cooldown
    if (!alreadyAttacked)
    {
        PerformAttack();

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
}

void PerformAttack()
{
    Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius, targetLayer);
     anim.SetBool("isAttacking", targetInAttackRange);
    foreach (Collider hit in hits)
    {
        PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
    void ResetAttack()
    {
        //Acción que resetea el ataque
        alreadyAttacked = false;
    }

    void CheckIfStuck()
    {
        //Acción que revisa si el agente está atrapado
        if (Time.time - lastCheckTime > stuckCheckTime)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);

            if (distanceMoved < stuckThreshold && agent.hasPath)
            {
                stuckTimer += stuckCheckTime;
            }
            else
            {
                stuckTimer = 0;
            }

            if (stuckTimer >= maxStuckDuration)
            {
                walkPointSet = false;
                agent.ResetPath();
                stuckTimer = 0;
            }

            lastPosition = transform.position;
            lastCheckTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return; //Solo se ejecutan los gizmos en editor de Unity

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
