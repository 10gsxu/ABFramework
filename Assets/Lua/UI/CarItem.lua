CarItem = LUIBase:New();
CarItem.__index = CarItem;

function CarItem:New()
    local self = {};
    setmetatable(self, CarItem);
    
    self.objDict = {};
    self.gameObject = self:Instantiate("UIPanel", "CarItem");
    self.transform = self.gameObject.transform;
    self:Awake();

    return self;
end

function CarItem:Awake()
    --遍历子对象，将子对象放入容器中
    GetChilds(self.transform, self.objDict);
    self.carImage = GetComponent(self.objDict["CarImage"], "Image");
end

function CarItem:Init(parent)
    self:SetParent(parent);
    self:Reset();
end

function CarItem:SetData(data)
    --local carName = CarData.GetProperty(tostring(data.carId), "neme");
    self.carImage.sprite = AssetLoader:LoadSprite("Car", tostring(data.carId));
end

function CarItem:MoveLeft(pos, time)
    self.transform.localPosition = pos;
    self.transform:DOLocalMoveX(pos.x-200, time/2);
    self.transform:DOLocalMoveX(pos.x, time/2):SetDelay(time/2);
end

function CarItem:MoveRight(pos, time)
    self.transform.localPosition = pos;
    self.transform:DOLocalMoveX(pos.x+200, time/2);
    self.transform:DOLocalMoveX(pos.x, time/2):SetDelay(time/2);
end

function CarItem:ProcessEvent(msg)

end