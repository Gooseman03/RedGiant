using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace UnityEngine.Sequences.Timeline
{
    /// <summary>
    /// A track you can use to control the active state of a Scene.
    /// </summary>
    [Serializable]
    [TrackClipType(typeof(SceneActivationPlayableAsset))]
    [TrackColor(0.55f, 0.5f, 0.14f)]
    public class SceneActivationTrack : TrackAsset
    {
        /// <summary>
        /// A reference to the Scene to control through this track.
        /// </summary>
        public SceneReference scene;

        List<GameObject> m_Buffer = new List<GameObject>();

        /// [doc-replica] com.unity.timeline
        /// <summary>
        /// Creates a mixer to blend playables generated by clips on the track.
        /// </summary>
        /// <param name="graph">The graph to inject playables into.</param>
        /// <param name="go">The GameObject that requested the graph.</param>
        /// <param name="inputCount">The number of playables from clips that will be inputs to the returned mixer.</param>
        /// <returns>A handle to the <see cref="Playable"/> that represents the mixer.</returns>
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<SceneActivationMixer> mixer = ScriptPlayable<SceneActivationMixer>.Create(graph, inputCount);
            mixer.GetBehaviour().SetData(scene);
            return mixer;
        }

        /// [doc-replica] com.unity.timeline
        /// <summary>
        /// The Timeline Editor calls this method to gather properties that require a preview.
        /// </summary>
        /// <param name="director">The PlayableDirector that invokes the preview.</param>
        /// <param name="driver">The PropertyCollector used to gather previewable properties.</param>
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            Scene loadedScene = SceneActivationManager.GetScene(scene.path);
            if (!loadedScene.isLoaded)
                return;

            m_Buffer.Clear();

            // TODO: this should be defined by the SceneActivationBehaviour.
            loadedScene.GetRootGameObjects(m_Buffer);

            foreach (GameObject root in m_Buffer)
                driver.AddFromName(root, "m_IsActive");
        }
    }
}