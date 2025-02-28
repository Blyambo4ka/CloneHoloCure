using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float moveSpeed = 2f;
    public float lifetime = 1f;

    public void SetText(int damage)
    {
        textMesh.text = damage.ToString();
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}
