using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnModelScaleModifier : MonoBehaviour
{
	#region Variables (private)

    [SerializeField]
    private Transform _modelsContainer;
    [SerializeField]
    private List<float> _scalesSteps = new List<float>() { 1f, 2f, 4f };

    private int _currStep = 0;

	#endregion

	#region Propiedades (public)

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {

    }

	/// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
	/// </summary>
	void Start()	
	{
        if (_modelsContainer == null) return;

        SetScaleModifier(GetModifier());
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update() 
	{
	
	}

    void OnClick()
    {
        if (_modelsContainer == null) return;

        // Avanza al siguiente step
        NextStep();

        // Cambia el scale
        SetScaleModifier(GetModifier());

        // Actualiza el texto
        var label = GetComponentInChildren<UILabel>();
        if (label != null)
        {
            label.text = "x" + GetModifier().ToString();
        }
    }

	#endregion

	#region Metodos

    public float GetModifier()
    {
        return _scalesSteps[_currStep];
    }

    private void NextStep()
    {
        if (++_currStep >= _scalesSteps.Count)
            _currStep = 0;
    }

    private void SetScaleModifier(float scaleModifier)
    {
        _modelsContainer.localScale = Vector3.one * scaleModifier;
    }

	#endregion
}

