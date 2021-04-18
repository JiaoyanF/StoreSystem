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
    RegEvent("goods:add_shop", Running.AddShop)
    RegEvent("goods:settle", Running.Settle)
    RegEvent("vip:data", Running.GetVipData)
    RegEvent("vip:add", Running.AddVip)
    RegEvent("vip:update", Running.UpdateVip)
    RegEvent("vip:delete", Running.DeleteVip)
    RegEvent("staff:data", Running.GetStaffData)
    RegEvent("staff:add", Running.AddStaff)
    RegEvent("staff:update", Running.UpdateStaff)
    RegEvent("staff:delete", Running.DeleteStaff)
    RegEvent("sales:record", Running.GetSalesRecord)
    RegEvent("sales:returns", Running.Returns)


# 触发事件
def FireEvent(tag, *args, **kwargs):
    if Events.get(tag) is None:
        return
    # print('事件参数类型：{0},事件参数:{1}'.format(type(args), args))
    if isinstance(args, tuple):
        if len(args) == 0:
            args = None
        elif len(args) == 1:
            args = args[0]
    if len(kwargs) == 0:
        return Events[tag](args)
    else:
        return Events[tag](kwargs['tar'], args)


# 注册事件
def RegEvent(tag, recv):
    Events[tag] = recv
