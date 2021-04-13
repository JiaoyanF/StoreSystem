# -*- coding: utf-8 -*-
"""
运行
"""
import Event
import Def


# class Running:
#     def Verify(self, args):
#         print("登录参数：{0}".format(args))


# 验证登录
def Verify(target, args):
    print("登录参数：{0}".format(args))
    if args is None:
        return
    cursor = Event.FireEvent("FindData", "Staff", 'id', args["username"]).fetchall()
    result = {'result': False}
    if len(cursor) == 0:
        result['reason'] = "user is null"
    elif cursor[0]['password'] == args["password"]:
        result['result'] = True
        result['user'] = cursor[0]
    else:
        result['reason'] = "password error"
    Event.FireEvent("SendMessage", "login:verify", result, tar=target)


# 获取商品数据
def GetGoodsData(target, args):
    result = {'type': "get", 'result': False}
    if type(args) is int:
        cursor = Event.FireEvent("FindData", "Goods", "id", args).fetchall()
    elif len(args) == 2:
        cursor = Event.FireEvent("FindData", "Goods", args).fetchall()
    else:
        cursor = Event.FireEvent("FindData", "Goods").fetchall()
    if len(cursor) > 0:
        result['result'] = True
        result['data'] = cursor
    Event.FireEvent("SendMessage", "goods:data", result, tar=target)


def AddGoods(target, data):
    result = {'type': "add", 'result': False, }
    # print("添加商品数据:{0}".format(data))
    if data is not None:
        result['result'], result['reason'] = Event.FireEvent("AddData", "Goods", data['Id'], data['Name'], data['Stock'], data['Type'], data['Price'], data['Tips'])
    else:
        result['result'] = False
        result['reason'] = "添加商品数据为空！"
    if result['result']:
        result['data'] = Event.FireEvent("FindData", "Goods", "id", data['Id']).fetchall()[0]
    Event.FireEvent("SendMessage", "goods:add", result, tar=target)


def DeleteGoods(target, data):
    result = {'type': "delete", 'result': False, }
    print("删除商品数据:{0}".format(data))
    result['result'], result['reason'] = Event.FireEvent("DeleteData", "Goods", "id", data)
    if result['result'] is True:
        result['id'] = data
    Event.FireEvent("SendMessage", "goods:delete", result, tar=target)


def UpdateGoods(target, data):
    result = {'type': "update", 'result': False}
    print("修改商品数据:{0}".format(data))
    now_data = Event.FireEvent("FindData", "Goods", "id", data['Id'])
    head_info = now_data.description  # 表头
    original = now_data.fetchall()[0]  # 原数据
    print("原数据:{0}".format(original))
    for field in head_info:
        if original[field[0]] == data[field[0].title()]:
            continue
        result['result'], result['reason'] = Event.FireEvent("UpData", "Goods", field[0], data[field[0].title()], str(data['Id']))
    if result['result']:
        result['data'] = Event.FireEvent("FindData", "Goods", "id", data['Id']).fetchall()[0]
    Event.FireEvent("SendMessage", "goods:update", result, tar=target)


def AddShop(target, data):
    result = {'type': "shop", 'result': False}
    item = Event.FireEvent("FindData", "Goods", "id", data['id']).fetchall()[0]
    if item['stock'] >= int(data['num']):
        result['result'] = True
        result['data'] = item
        result['num'] = data['num']
    else:
        result['result'] = False
        result['reason'] = "库存不足！"
    Event.FireEvent("SendMessage", "goods:add_shop", result, tar=target)