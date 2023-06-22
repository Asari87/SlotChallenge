namespace Game.Core.SceneManagment
{
    public interface IHandleScenes
    {
        /// <summary>
        /// Load scebe by scene index
        /// </summary>
        /// <param name="index"></param>
        public void LoadScene(int index, bool addtive = false);
        /// <summary>
        /// Load scene by name
        /// </summary>
        /// <param name="index"></param>
        public void SaveScene(string name, bool addtive = false);
    }
}