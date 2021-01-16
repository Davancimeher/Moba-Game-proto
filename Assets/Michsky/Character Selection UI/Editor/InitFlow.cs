using UnityEditor;

namespace Michsky.UI.Flow
{
    public class InitMUIP
    {
        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                if (!EditorPrefs.HasKey("FlowUI.Installed"))
                {
                    EditorPrefs.SetInt("FlowUI.Installed", 1);
                    EditorUtility.DisplayDialog("Hello there!", "Thank you for purchasing Flow UI.\r\rFirst of all, import TextMesh Pro from Package Manager if you haven't already." +
                        "\r\rYou can manage your UI by clicking Window > Tools > Flow UI > Show UI Manager.\r\rYou can contact me at support@michsky.com for support.", "Got it!");
                }
            }
        }
    }
}