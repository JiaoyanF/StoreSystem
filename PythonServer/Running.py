# -*- coding: utf-8 -*-
"""
运行
"""
import Event
import Def
import time


# class Running:
#     def Verify(self, args):
#         print("登录参数：{0}".format(args))

# 获取数据
def GetData(list_name, primary_key=None):
    if primary_key is None:
        return Event.FireEvent("FindData", list_name).fetchall()
    else:
        return Event.FireEvent("FindData", list_name, "id", primary_key).fetchall()


# 删除数据
def DelData(list_name, primary_key=None):
    if primary_key is None:
        return Event.FireEvent("DeleteData", list_name).fetchall()
    else:
        return Event.FireEvent("DeleteData", list_name, "id", primary_key).fetchall()


# 验证登录
def Verify(target, args):
    print("登录参数：{0}".format(args))
    if args is None:
        return
    cursor = GetData("Staff", args["username"])
    result = {'result': False}
    if len(cursor) == 0:
        result['reason'] = "user is null"
    elif cursor[0]['password'] == args["password"]:
        result['result'] = True
        result['user'] = cursor[0]
        Def.Threads[target]["user"] = cursor[0]
    else:
        result['reason'] = "password error"
    Event.FireEvent("SendMessage", "login:verify", result, tar=target)


# 获取商品数据
def GetGoodsData(target, args=None):
    result = {'type': "get", 'result': False}
    print("获取商品数据：{0}".format(args))
    if args is not None:
        cursor = GetData("Goods", args)
    else:
        cursor = GetData("Goods")
    if len(cursor) > 0:
        result['result'] = True
        result['data'] = cursor
    Event.FireEvent("SendMessage", "goods:data", result, tar=target)


# 添加商品
def AddGoods(target, data):
    result = {'type': "add", 'result': False, }
    # print("添加商品数据:{0}".format(data))
    if data is not None:
        result['result'], result['reason'] = Event.FireEvent("AddData", "Goods", data['Id'], data['Name'], data['Stock'], data['Type'], data['Price'], data['Tips'])
    else:
        result['result'] = False
        result['reason'] = "添加商品数据为空！"
    if result['result']:
        result['data'] = GetData("Goods", data['Id'])[0]
    Event.FireEvent("SendMessage", "goods:add", result, tar=target)


# 删除商品
def DeleteGoods(target, data):
    result = {'type': "delete", 'result': False, }
    print("删除商品数据:{0}".format(data))
    result['result'], result['reason'] = DelData("DeleteData", data)
    if result['result'] is True:
        result['id'] = data
    Event.FireEvent("SendMessage", "goods:delete", result, tar=target)


# 修改商品
def UpdateGoods(target, data):
    result = {'type': "update", 'result': False}
    print("修改商品数据:{0}".format(data))
    now_data = GetData("Goods", data['Id'])
    head_info = now_data.description  # 表头
    original = now_data.fetchall()[0]  # 原数据
    print("原数据:{0}".format(original))
    for field in head_info:
        if original[field[0]] == data[field[0].title()]:
            continue
        result['result'], result['reason'] = Event.FireEvent("UpData", "Goods", field[0], data[field[0].title()], str(data['Id']))
    if result['result']:
        result['data'] = GetData("Goods", data['Id'])[0]
    Event.FireEvent("SendMessage", "goods:update", result, tar=target)


# 添加商品到购物清单
def AddShop(target, data):
    result = {'type': "shop", 'result': False}
    item = GetData("Goods", data['id'])[0]
    if item['stock'] >= int(data['num']):
        result['result'] = True
        result['data'] = item
        result['num'] = data['num']
    else:
        result['result'] = False
        result['reason'] = "库存不足！"
    Event.FireEvent("SendMessage", "goods:add_shop", result, tar=target)


# 结账
def Settle(target, data):
    result = {'type': "settle", 'result': False}
    print("结算数据:{0}".format(data))
    record = []
    money = 0
    # 整理购买清单
    for item in data['SalesList']:
        print(item)
        find = GetData("Goods", item['Id'])[0]
        if find['stock'] < item['Num']:
            result['reason'] = "购买列表中有商品库存不足!"
            break
        money = money + item['Num'] * item['Price']
        arg = "'" + str(item['Id']) + ":" + str(item['Num']) + "'"
        record.append(arg)
        result['result'], result['reason'] = Event.FireEvent("UpData", "Goods", "stock", find['stock'] - item['Num'], str(item['Id']))
    record_str = ",".join(record)
    print("销售清单:{0}".format(record_str))
    index = len(GetData("SalesRecord")) + 1
    now_time = int(time.time())
    member_id = 0
    if data['Member'] is not None:
        member_id = data['Member']['Id']
    result['result'], result['reason'] = Event.FireEvent("AddData", "SalesRecord", index, now_time, money, Def.Threads[target]["user"]["id"], member_id)

    Event.FireEvent("SendMessage", "goods:settle", result, tar=target)


# 获取销售清单
def GetSalesRecord(target, args):
    result = {'type': "get", 'result': False}
    print("获取销售数据：{0}".format(args))
    if args is not None:
        cursor = GetData("SalesRecord", args)
    else:
        cursor = GetData("SalesRecord")
    if len(cursor) > 0:
        result['result'] = True
        result['data'] = cursor
    Event.FireEvent("SendMessage", "goods:data", result, tar=target)


# 获取员工数据
def GetStaffData(target, data):
    pass
