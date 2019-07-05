SettingData = IJsonData:New()
local this = SettingData

function SettingData.Init()
    this:InitData("SettingData");
end

function SettingData.Save()
    this:SaveData()
end

function SettingData.GetSoundVolume()
    return this.fileData.settingInfo.sound
end

function SettingData.SetSoundVolume(volume)
    this.fileData.settingInfo.sound = volume
end

function SettingData.GetMusicVolume()
    return this.fileData.settingInfo.music
end

function SettingData.SetMusicVolume(volume)
    this.fileData.settingInfo.music = volume
end