using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    [SerializeField]
    BallController.Settings animationSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(animationSettings);
    }
}