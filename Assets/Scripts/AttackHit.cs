using UnityEngine;

     public class FVXmelee : MonoBehaviour
     {
         public Transform target; // Цель, за которой нужно следовать (ваш персонаж)
         public Vector3 offset; // Смещение от цели

        public bool followRotation = false; // Следовать повороту

        void LateUpdate()
         {
             if (target == null) return;
              // устанавливаем позицию
             transform.position = target.position + offset;
                // устанавливаем поворот
            if (followRotation) {
             transform.rotation = target.rotation;
            }
         }
     }