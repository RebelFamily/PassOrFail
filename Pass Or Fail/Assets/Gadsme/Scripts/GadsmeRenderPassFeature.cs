
using UnityEngine;
using UnityEngine.Rendering;

namespace Gadsme
{
    public class GadsmeRenderPassFeature : UnityEngine.Rendering.Universal.ScriptableRendererFeature
    {
#if GADSME_URP_RENDER_PASS
        private GadsmeRenderPass gadsmeRenderPass = null;
#endif

        public override void Create()
        {
#if GADSME_URP_RENDER_PASS
            gadsmeRenderPass = new GadsmeRenderPass();
#endif
        }

        public override void AddRenderPasses(UnityEngine.Rendering.Universal.ScriptableRenderer renderer, ref UnityEngine.Rendering.Universal.RenderingData renderingData)
        {
#if GADSME_URP_RENDER_PASS
            if (gadsmeRenderPass == null)
                return;

            gadsmeRenderPass.AttachBindings();

            if (gadsmeRenderPass != null && gadsmeRenderPass.impl.Md0ad18db7ee98268(renderingData.cameraData.camera))
                renderer.EnqueuePass(gadsmeRenderPass);
#endif
        }
    }

#if GADSME_URP_RENDER_PASS
    public class GadsmeRenderPass : UnityEngine.Rendering.Universal.ScriptableRenderPass
    {
        internal Te956008dee95d512 impl = new Te956008dee95d512();

        private ScriptableRenderContext SRC;

        public GadsmeRenderPass()
        {
            this.renderPassEvent = UnityEngine.Rendering.Universal.RenderPassEvent.AfterRenderingTransparents;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            impl.M9ac82470623cf06a();
        }

        public override void Execute(ScriptableRenderContext context, ref UnityEngine.Rendering.Universal.RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(impl.Mb6888842c642aeab());
            impl.M459eaaeec1e94c54(cmd);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        public void AttachBindings()
        {
            if (!impl.Mae80d0b8688f8964())
                return;

            impl.M9a557fd2383c3138(() =>
            {
                UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
                UnityEngine.Rendering.RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
            });

            impl.M371d024a2bf6d681(() =>
            {
                UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            });

            impl.M123f5e0dd966d2b3(() =>
            {
                UnityEngine.Rendering.RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
            });

            impl.M00cfca9468258577((camera) =>
            {
                UnityEngine.Rendering.Universal.UniversalRenderPipeline.RenderSingleCamera(SRC, camera);
            });
        }

        private void OnBeginCameraRendering(ScriptableRenderContext SRC, Camera camera)
        {
            this.SRC = SRC;
            impl.Md4dc8e708e665554(camera);
        }

        private void OnEndCameraRendering(ScriptableRenderContext SRC, Camera camera)
        {
            this.SRC = SRC;
            impl.Mbbea0ffbadc88b53(camera);
        }
    }
#endif
}
