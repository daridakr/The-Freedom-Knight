using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vendor : NPC, IInteractable
{
    [SerializeField]
    private VendorItem[] items;

    public VendorItem[] Items { get => items; }
}
