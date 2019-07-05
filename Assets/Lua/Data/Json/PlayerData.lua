PlayerData = IJsonData:New()
local this = PlayerData

function PlayerData.Init()
    this:InitData("PlayerData");
end

function PlayerData.Save()
    this:SaveData();
end

--金币
function PlayerData.GetCoinCount()
    return this.fileData.playerInfo.coin
end

function PlayerData.SetCoinCount(coinCount)
    this.fileData.playerInfo.coin = coinCount
    EventHandler.emit("CoinCount", coinCount)
end

function PlayerData.AddCoinCount(addCount)
    this.fileData.playerInfo.coin = this.fileData.playerInfo.coin + addCount
    EventHandler.emit("CoinCount", this.fileData.playerInfo.coin)
end

--宝石
function PlayerData.GetJewelCount()
    return this.fileData.playerInfo.jewel
end

function PlayerData.SetJewelCount(jewelCount)
    this.fileData.playerInfo.jewel = jewelCount
    EventHandler.emit("JewelCount", jewelCount)
end

function PlayerData.AddJewelCount(addCount)
    this.fileData.playerInfo.jewel = this.fileData.playerInfo.jewel + addCount
    EventHandler.emit("JewelCount", this.fileData.playerInfo.jewel)
end

--经验
function PlayerData.GetExpCount()
    return this.fileData.playerInfo.exp
end

function PlayerData.SetExpCount(expCount)
    this.fileData.playerInfo.exp = expCount
    EventHandler.emit("ExpCount", this.fileData.playerInfo.exp)
end

function PlayerData.AddExpCount(addCount)
    this.fileData.playerInfo.exp = this.fileData.playerInfo.exp + addCount
    EventHandler.emit("ExpCount", this.fileData.playerInfo.exp)
end

--玩家等级
function PlayerData.GetPlayerLevel()
    return this.fileData.playerInfo.playerLevel
end

function PlayerData.SetPlayerLevel(level)
    this.fileData.playerInfo.playerLevel = level
    EventHandler.emit("PlayerLevel", level)
end

function PlayerData.UpgradePlayerLevel()
    this.fileData.playerInfo.playerLevel = this.fileData.playerInfo.playerLevel + 1
    EventHandler.emit("PlayerLevel", this.fileData.playerInfo.playerLevel)
end

--游戏关卡
function PlayerData.GetGameLevel()
    return this.fileData.playerInfo.gameLevel
end

function PlayerData.SetGameLevel(level)
    this.fileData.playerInfo.gameLevel = level
    EventHandler.emit("GameLevel", level)
end

function PlayerData.UpgradeGameLevel()
    this.fileData.playerInfo.gameLevel = this.fileData.playerInfo.gameLevel + 1
    EventHandler.emit("GameLevel", this.fileData.playerInfo.gameLevel)
end

--最大距离
function PlayerData.GetMaxDistance()
    return this.fileData.playerInfo.maxDistance
end

function PlayerData.SetMaxDistance(distance)
    this.fileData.playerInfo.maxDistance = distance
    EventHandler.emit("MaxDistance", distance)
end

--游戏使用的CarId
function PlayerData.GetGameCarId()
    return this.fileData.playerInfo.gameCarId
end

function PlayerData.SetGameCarId(carId)
    this.fileData.playerInfo.gameCarId = carId
    EventHandler.emit("GameCarId", carId)
end

--玩家最高等级的CarId
function PlayerData.GetMaxCarId()
    return this.fileData.playerInfo.maxCarId
end

function PlayerData.SetMaxCarId(carId)
    this.fileData.playerInfo.maxCarId = carId
    EventHandler.emit("MaxCarId", carId)
end

--车位列表
function PlayerData.GetSeatList()
    return this.fileData.playerInfo.seatList
end