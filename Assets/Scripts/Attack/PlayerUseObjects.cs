using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseObjects : MonoBehaviour
{
    public delegate void Use(Vector2 direction);
    public event Use OnUse = delegate { };

    [SerializeField][Min(0)] private float cooldownDuration = 1;

    private Camera camera;
    private float cooldownTime = 0;

    private void Awake()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gamePaused || GameManager.instance.PlayerMovementDisabled) return;

        if (InputManager.instance.GetKeyDown(InputActions.UseTool))
        {
            if (GameTime.time > cooldownTime)
            {
                OnUse(transform.position - camera.ViewportToWorldPoint(Input.mousePosition));
                cooldownTime = GameTime.time + cooldownDuration;
            }
        }
    }

    public void SetCooldown(float cooldown)
    {
        cooldownDuration = cooldown;
    }
}
