AudioManager = {}
local this = AudioManager

function AudioManager.Init()
    this.gameObject = GameObject.Instantiate(AssetLoader:LoadObject("Audio", "AudioContainer"));
    this.transform = this.gameObject.transform;
    Object.DontDestroyOnLoad(this.gameObject);
end

function AudioManager.PlaySound(audioId)
    AudioController.Play(audioId, this.transform, 1)
end

function AudioManager.PlayMusic(audioId)
    AudioController.PlayMusic(audioId, this.transform);
end

function AudioManager.StopSound(audioId)
    AudioController.Stop(audioId)
end

function AudioManager.StopMusic(audioId)
    AudioController.StopMusic();
end

function AudioManager.SetSoundState(state)
    if state then
        this.SetSoundState(100)
    else
        this.SetSoundState(0)
    end
end

function AudioManager.SetMusicState(state)
    if state then
        this.SetMusicVolume(100)
    else
        this.SetMusicVolume(0)
    end
end

function AudioManager.SetSoundVolume(volume)
    AudioController.GetCategory("Sound").Volume = volume;
end

function AudioManager.SetMusicVolume(volume)
    AudioController.GetCategory("Music").Volume = volume;
end