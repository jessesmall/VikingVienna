using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJump {

    float JumpForce { get; set; }
    bool HasDoubleJumped { get; set; }
    bool CanDoubleJump { get; set; }

    void HandleJump();
}
