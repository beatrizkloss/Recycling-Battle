using UnityEngine;
using System.Collections;
public class DestruirAposAnimacao : MonoBehaviour
{
    public Animator animator;

   private void Start()
{
    if (animator == null)
    {
        Debug.LogError("Animator not assigned in DestruirAposAnimacao script on " + gameObject.name);
        return; // Exit the method if animator is not assigned
    }

    // Obter a duração da animação e iniciar a coroutine
    StartCoroutine(DestruirDepoisDeAnimacao(animator.GetCurrentAnimatorStateInfo(0).length));
}
    private IEnumerator DestruirDepoisDeAnimacao(float duracao)
    {
        yield return new WaitForSeconds(duracao);
        Destroy(gameObject);
    }
}
