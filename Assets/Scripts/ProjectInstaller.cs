using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    GameObject soundControllerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<SoundController>().FromComponentInNewPrefab(soundControllerPrefab).AsSingle();
    }
}