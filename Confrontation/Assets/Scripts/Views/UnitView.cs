using System;
using Data;
using Entities;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class UnitView : BaseView
    {
        [SerializeField] private AnimatorOverrideController[] _overrideControllers;
        
        private Animator _animator;
        
        public UnitEntity UnitEntity;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        }

        public void LookAt(Vector3 target)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = transform.position.x > target.x;
        }

        public void SetAnimations(int number)
        {
            _animator.runtimeAnimatorController = _overrideControllers[number];
        }

        public void PlayMoveAnim()
        {
            _animator.SetBool("Grounded", true); 
            _animator.SetInteger("AnimState", 1);
        }
    }
}
