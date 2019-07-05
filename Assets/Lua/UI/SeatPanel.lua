SeatPanel = LUIBase:New();
local this = SeatPanel;

function SeatPanel.Awake()
    local eventTriggerListener = this:GetUIComponent("MainPanel.BtnCar", "EventTriggerListener");
    eventTriggerListener.onBeginDrag = eventTriggerListener.onBeginDrag + this.onBeginDrag;
    eventTriggerListener.onDrag = eventTriggerListener.onDrag + this.onDrag;
    eventTriggerListener.onEndDrag = eventTriggerListener.onEndDrag + this.onEndDrag;

    this.radiu = 60;
    this.seatList = PlayerData.GetSeatList();
    this.seatItemList = {};
    this.beginIndex = nil;
    this.endIndex = nil;
    this.isActive = false;

    this.canvas = Canvas:GetComponent("Canvas");

    this.carContainer = this:GetGameObject("MainPanel.CarContainer").transform;
    local playerLevel = PlayerData.GetPlayerLevel();
    this.positionList = PositionData.GetPositionList("Level"..playerLevel);
    this.trashPosition = PositionData.GetTrashPosition();
    this.seatCount = #this.positionList;
    for i=1, this.seatCount, 1 do
        local seatItem = GameObjectFactory.Create("SeatItem");
        seatItem:Init(this.carContainer, i);
        seatItem:SetName("SeatItem"..i);
        seatItem:SetPosition(Vector3.New(this.positionList[i].x, this.positionList[i].y, 0));
        seatItem:SetData(this.seatList[i]);
        this.seatItemList[i] = seatItem;
    end
end

--获取相应的车index
function SeatPanel.getIndex(pos)
    local distance = Distance(pos, this.trashPosition);
    if distance < this.radiu then
        return 0;--0表示删除
    end
    for i=1, this.seatCount, 1 do
        local distance = Distance(pos, this.positionList[i]);
        if distance < this.radiu then
            return i;
        end
    end
    return -1;
end

function SeatPanel.getPosition(position)
    local inRect, pos = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvas.transform, position, this.canvas.worldCamera, nil);
    return pos;
end

function SeatPanel.onBeginDrag(eventData)
    local pos = this.getPosition(eventData.position);
    this.beginIndex = this.getIndex(pos);

    this.isActive = this.beginIndex > 0 and this.seatList[this.beginIndex].carId > 0;
    if this.isActive == false then
        return;
    end

    this.carItem = GameObjectFactory.Create("CarItem");
    this.carItem:Init(this.carContainer);
    this.carItem:SetPosition(Vector3.New(pos.x, pos.y, 0));
    this.carItem:SetData(this.seatList[this.beginIndex]);
    this.seatItemList[this.beginIndex]:SetState(true);
end

function SeatPanel.onDrag(eventData)
    if this.isActive == false then
        return;
    end

    local pos = this.getPosition(eventData.position);
    this.carItem:SetPosition(Vector3.New(pos.x, pos.y, 0));
end

function SeatPanel.onEndDrag(eventData)
    if this.isActive == false then
        return;
    end
    local pos = this.getPosition(eventData.position);
    this.endIndex = this.getIndex(pos);

    this.seatItemList[this.beginIndex]:SetState(false);
    GameObjectFactory.Recycle("CarItem", this.carItem);

    --无效
    if this.endIndex == -1 then
        return;
    end

    --删除
    if this.endIndex == 0 then
        print("delete");
        return;
    end

    --交换位置或者升级
    if this.seatList[this.beginIndex].carId ~= this.seatList[this.endIndex].carId then
        this.Exchange();
    else
        this.Upgrade();
    end
end

function SeatPanel.Exchange()
    this.seatList[this.beginIndex], this.seatList[this.endIndex] = this.seatList[this.endIndex], this.seatList[this.beginIndex];
    this.seatItemList[this.beginIndex]:SetData(this.seatList[this.beginIndex]);
    this.seatItemList[this.endIndex]:SetData(this.seatList[this.endIndex]);
    PlayerData.Save();
end

function SeatPanel.Upgrade()
    local pos = this.positionList[this.endIndex];
    this.leftCarItem = GameObjectFactory.Create("CarItem");
    this.leftCarItem:Init(this.carContainer);
    this.leftCarItem:SetData(this.seatList[this.endIndex]);
    this.leftCarItem:MoveLeft(pos, 0.4);
    this.rightCarItem = GameObjectFactory.Create("CarItem");
    this.rightCarItem:Init(this.carContainer);
    this.rightCarItem:SetData(this.seatList[this.endIndex]);
    this.rightCarItem:MoveRight(pos, 0.4);
    local timer = Timer.New(this.Recycle, 0.4);
    timer:Start();
end

function SeatPanel.Recycle()
    GameObjectFactory.Recycle("CarItem", this.leftCarItem);
    GameObjectFactory.Recycle("CarItem", this.rightCarItem);
end

function SeatPanel.SeatOnClick(go)
    print(go.name);
    local msg = LPanelMsg:New(LUIEvent.ShowPanel, "RankPanel");
    this:SendMsg(msg);
end