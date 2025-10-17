using UnityEngine;

public class DebugCollision : MonoBehaviour
{
    // Esta função é chamada quando outro collider entra num collider marcado como "Is Trigger"
    private void OnTriggerEnter(Collider other)
    {
        // Imprime no console o nome do objeto que atravessou
        Debug.Log($"<color=green>SUCESSO!</color> O objeto <b>'{other.gameObject.name}'</b> atravessou a porta.");
    }

    // Esta função é chamada para colisões físicas sólidas (ambos sem "Is Trigger")
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"<color=red>COLISÃO FÍSICA!</color> O objeto <b>'{collision.gameObject.name}'</b> bateu na porta.");
    }
}