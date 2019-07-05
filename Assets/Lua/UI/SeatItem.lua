SeatItem = LUIBase:New();
SeatItem.__index = SeatItem;

function SeatItem:New()
    local self = {};
    setmetatable(self, SeatItem);
    
    self.objDict = {};
    self.gameObject = self:Instantiate("UIPanel", "SeatItem");
    self.transform = self.gameObject.transform;

    return self;
end

function SeatItem:Init(parent, index)
    self:SetParent(parent);
    self:Reset();

    self.index = index;
    --遍历子对象，将子对象放入容器中
    GetChilds(self.transform, self.objDict);

    self.carImage = GetComponent(self.objDict["CarImage"], "Image");
end

function SeatItem:SetData(data)
    --local carName = CarData.GetProperty(tostring(data.carId), "neme");
    self.carImage.gameObject:SetActive(data.carId ~= 0);
    if data.carId ~= 0 then
        self.carImage.sprite = AssetLoader:LoadSprite("Car", tostring(data.carId));
    end
end

function SeatItem:SetState(state)
    local color;
    if state then
        color = Color.New(1, 1, 1, 0.3);
    else
        color = Color.New(1, 1, 1, 1);
    end
    self.carImage.color = color;
end

function SeatItem:ProcessEvent(msg)

end