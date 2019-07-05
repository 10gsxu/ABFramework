EventHandler = {}
local this = EventHandler;

function EventHandler.on(key, func, target)
    if this.key == nil then
        this.key = {}
    end
    table.insert(this.key, {func = func, target = target})
end

function EventHandler.emit(key, param)
    if this.key == nil then
        return
    end
    for k, v in pairs(this.key) do
        if v.target == nil then
            v.func(param);
        else
            v.func(v.target, param)
        end
    end
end

function EventHandler.remove(key, func, target)
    if this.key == nil then
        return
    end
    local index = 0
    for k, v in pairs(this.key) do
        if v.func == func and v.target == target then
            index = k
        end
    end
    if index == 0 then
        print("func不在key中")
    else
        table.remove(this.key, index)
    end
end

function EventHandler.clear(key)
    this.key = nil
end