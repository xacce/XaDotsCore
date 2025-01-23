using Unity.Entities;
using Unity.Scenes;

namespace Core.Runtime.SystemGroups.EntityCreationLifecycle
{
    // Смысл - облегчиь цикл создания и настройки сущностей

    // Реализация: В конце симуляции (после ecb) планировать создание сущностей на след. кадр т.е писать в  BeginInitilizationEcb
    // В начале след. кадр внутри EarlyEntitySetupSystem выполнять все необходимые задачи по первичной настройке

    // Важно:
    // PostLateEntityCreationSystem использовать BeginIni ecb
    // EarlyEntitySetupSystem использовать EndIni ecb

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial class PostLateEntityCreationSystem : ComponentSystemGroup
    {
    }


    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SceneSystemGroup))]
    public partial class EarlyEntitySetupSystem : ComponentSystemGroup
    {
    }
}