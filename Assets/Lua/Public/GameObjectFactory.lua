--工厂类，生成&回收Item预设，返回值&回收值都是绑定的luaScript
GameObjectFactory = {
    objDict = {}
}
GameObjectFactory.__index = GameObjectFactory;
local this = GameObjectFactory;

function GameObjectFactory.Init()
    this.scriptDict = {
        ShopItem = ShopItem,
        CarItem = CarItem,
        SeatItem = SeatItem
    };
end

function GameObjectFactory.Create(assetName)
    local luaScript = nil;
    if this.objDict[assetName] == nil then
        this.objDict[assetName] = {};
    end
    local assetTable = this.objDict[assetName];
    if #assetTable > 0 then
        --print("#assetTable : " .. #assetTable);
        luaScript = assetTable[1];
        table.remove(assetTable, 1);--删除第一个元素，默认删除最后一个
        --print("#assetTable : " .. #assetTable);
    else
        luaScript = this.scriptDict[assetName]:New();
    end
    luaScript:SetActive(true);
    return luaScript;
end

function GameObjectFactory.Recycle(assetName, luaScript)
    luaScript:SetActive(false);
    table.insert(this.objDict[assetName], luaScript);
end