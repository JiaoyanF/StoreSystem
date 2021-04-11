# -*- coding: utf-8 -*-
"""
事件
"""
import DBControl
import Connect
import Running
import Def
Events = {}


# 初始化事件
def InitEvent():
    # 本地事件
    RegEvent("AddData", DBControl.AddData)
    RegEvent("UpData", DBControl.UpData)
    RegEvent("FindData", DBControl.FindData)
    RegEvent("DeleteData", DBControl.DeleteData)
    RegEvent("SendMessage", Connect.SendMessage)
    # 网络事件
    RegEvent("login:verify", Running.Verify)
    RegEvent("goods:data", Running.GetGoodsData)
    RegEvent("goods:add", Running.AddGoods)
    RegEvent("goods:delete", Running.DeleteGoods)
    RegEvent("goods:update", Running.UpdateGoods)



# 触发事件
def FireEvent(tag, *args, **kwargs):
    if Events.get(tag) is None:
        return
    if len(args) == 1:
        args = args[0]
    print('事件参数：{0}'.format(args))
    if len(kwargs) == 0:
        return Events[tag](args)
    else:
        return Events[tag](kwargs['tar'], args)


# 注册事件
def RegEvent(tag, recv):
    Events[tag] = recv
