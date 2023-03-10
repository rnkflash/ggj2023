using System.Threading.Tasks;

using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
	public async void StartGame()
	{
		await LoadSceneAsync("menu", LoadSceneMode.Single);
	}

	public async void LoadMainMenu()
	{
		Player.Instance.ResetProgress();
		await LoadSceneAsync("menu", LoadSceneMode.Single);
	}

	public async void LoadGameplay()
	{
		await LoadSceneAsync("level2", LoadSceneMode.Single);
	}

	public async void LoadDeathScreen()
	{
		await LoadSceneAsync("lose", LoadSceneMode.Single);
	}

	private async Task LoadSceneAsync(string sceneName, LoadSceneMode mode)
	{
		var loadOp = SceneManager.LoadSceneAsync(sceneName, mode);

		while (!loadOp.isDone)
		{
			await Task.Delay(60);
		}
	}
}