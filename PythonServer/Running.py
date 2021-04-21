# -*- coding: utf-8 -*-
"""
运行
"""
import datetime

import Event
import Def
import time


# class Running:
#     def Verify(self, args):
#         print("登录参数：{0}".format(args))

# 获取数据
def GetData(list_name, primary_key=None):
    if primary_key is None:
        return Event.FireEvent("FindData", list_name)
    else:
        return Event.FireEvent("FindData", list_name, "id", primary_key)


# 删除数据
def DelData(list_name, primary_key=None):
    if primary_key is None:
        return Event.FireEvent("DeleteData", list_name)
    else:
        return Event.FireEvent("DeleteData", list_name, "id", primary_key)


# 存储操作记录
def KeepRecords(target, list_name, obj_id, operation):
    record_index = len(GetData(list_name).fetchall()) + 1
    now_time = "%s,%s,%s" % (datetime.datetime.now().year, datetime.datetime.now().month, datetime.datetime.now().day)
    staff = Def.Threads[target]["user"]["id"]
    Event.FireEvent("AddData", list_name, record_index, now_time, obj_id, staff, operation)


# 验证登录
def Verify(target, args):
    # print("登录参数：{0}".format(args))
    if args is None:
        return
    cursor = GetData("Staff", args["username"]).fetchall()
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
    # print("获取商品数据：{0}".format(args))
    if args is not None:
        cursor = GetData("Goods", args).fetchall()
    else:
        cursor = GetData("Goods").fetchall()
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
        result['data'] = GetData("Goods", data['Id']).fetchall()[0]
        # 添加到商品记录
        KeepRecords(target, "GoodsRecord", data['Id'], "上货")
    Event.FireEvent("SendMessage", "goods:add", result, tar=target)


# 删除商品
def DeleteGoods(target, data):
    result = {'type': "delete", 'result': False, }
    # print("删除商品数据:{0}".format(data))
    result['result'], result['reason'] = DelData("Goods", data)
    if result['result'] is True:
        result['id'] = data
        # 添加到商品记录
        KeepRecords(target, "GoodsRecord", data, "下架")
    Event.FireEvent("SendMessage", "goods:delete", result, tar=target)


# 修改商品
def UpdateGoods(target, data):
    result = {'type': "update", 'result': False}
    # print("修改商品数据:{0}".format(data))
    now_data = GetData("Goods", data['Id'])
    head_info = now_data.description  # 表头
    original = now_data.fetchall()[0]  # 原数据
    # print("原数据:{0}".format(original))
    for field in head_info:
        if original[field[0]] == data[field[0].title()]:
            continue
        result['result'], result['reason'] = Event.FireEvent("UpData", "Goods", field[0], data[field[0].title()], str(data['Id']))
    if result['result']:
        result['data'] = GetData("Goods", data['Id']).fetchall()[0]
        # 添加到商品记录
        KeepRecords(target, "GoodsRecord", data['Id'], "修改")
    Event.FireEvent("SendMessage", "goods:update", result, tar=target)


# 添加商品到购物清单
def AddShop(target, data):
    result = {'type': "shop", 'result': False}
    item = GetData("Goods", data['id']).fetchall()
    if len(item) == 1:
        item = item[0]
        if item['stock'] >= int(data['num']):
            result['result'] = True
            result['data'] = item
            result['num'] = data['num']
        else:
            result['result'] = False
            result['reason'] = "库存不足！"
    else:
        result['result'] = False
        result['reason'] = "没有该商品！"
    Event.FireEvent("SendMessage", "goods:add_shop", result, tar=target)


# 结账
def Settle(target, data):
    result = {'type': "settle", 'result': False}
    # print("结算数据:{0}".format(data))
    record = []
    money = 0
    # 整理购买清单
    for item in data['SalesList']:
        # print(item)
        find = GetData("Goods", item['Id']).fetchall()[0]
        if find['stock'] < item['Num']:
            result['reason'] = "购买列表中有商品库存不足!"
            break
        money = money + item['Num'] * item['Price']
        arg = str(item['Id']) + "," + str(item['Num']) + "," + str(item['Price']) + "," + str(item['Name']) + "," + '0'
        record.append(arg)
        result['result'], result['reason'] = Event.FireEvent("UpData", "Goods", "stock", find['stock'] - item['Num'], str(item['Id']))
    record_str = ";".join(record)
    # print("销售清单:{0}".format(record_str))
    index = len(GetData("SalesRecord").fetchall()) + 1
    now_time = "%s,%s,%s" % (datetime.datetime.now().year, datetime.datetime.now().month, datetime.datetime.now().day)
    member_id = data['Vip']
    result['result'], result['reason'] = Event.FireEvent("AddData", "SalesRecord", index, now_time, str(record_str), money, Def.Threads[target]["user"]["id"], member_id)

    Event.FireEvent("SendMessage", "goods:settle", result, tar=target)


# 获取会员数据
def GetVipData(target, args):
    result = {'type': "get", 'result': False}
    # print("获取会员数据：{0}".format(args))
    if args is not None:
        cursor = GetData("Vip", args).fetchall()
    else:
        cursor = GetData("Vip").fetchall()
    if len(cursor) > 0:
        result['result'] = True
        result['data'] = cursor
    else:
        result['reason'] = "会员为空"
    Event.FireEvent("SendMessage", "vip:data", result, tar=target)


# 添加会员
def AddVip(target, data):
    result = {'type': "add", 'result': False}
    # print("添加会员数据:{0}".format(data))
    if data is not None:
        join = "%s,%s,%s" % (datetime.datetime.now().year, datetime.datetime.now().month, datetime.datetime.now().day)
        result['result'], result['reason'] = Event.FireEvent("AddData", "Vip", data['Id'], data['Name'], data['Point'], data['Gender'], data['Birth'], join)
    else:
        result['result'] = False
        result['reason'] = "添加数据为空！"
    if result['result']:
        result['data'] = GetData("Vip", data['Id']).fetchall()[0]
        KeepRecords(target, "VipRecord", data['Id'], "添加")
    Event.FireEvent("SendMessage", "vip:add", result, tar=target)


# 删除会员
def DeleteVip(target, data):
    result = {'type': "delete", 'result': False, }
    # print("删除会员数据:{0}".format(data))
    result['result'], result['reason'] = DelData("Vip", data)
    if result['result'] is True:
        result['id'] = data
        KeepRecords(target, "VipRecord", data, "删除")
    Event.FireEvent("SendMessage", "vip:delete", result, tar=target)


# 修改会员
def UpdateVip(target, data):
    result = {'type': "update", 'result': False}
    # print("修改会员数据:{0}".format(data))
    now_data = GetData("Vip", data['Id'])
    head_info = now_data.description  # 表头
    original = now_data.fetchall()[0]  # 原数据
    # print("原数据:{0}".format(original))
    for field in head_info:
        if original[field[0]] == data[field[0].title()] or field[0] == "enter":
            continue
        result['result'], result['reason'] = Event.FireEvent("UpData", "Vip", field[0], data[field[0].title()], str(data['Id']))
    if result['result']:
        result['data'] = GetData("Vip", data['Id']).fetchall()[0]
        KeepRecords(target, "VipRecord", data['Id'], "修改")
    Event.FireEvent("SendMessage", "vip:update", result, tar=target)


# 获取销售清单
def GetSalesRecord(target, args):
    result = {'type': "get", 'result': False}
    # print("获取销售数据：{0}".format(args))
    if args is not None:
        cursor = GetData("SalesRecord", args).fetchall()
    else:
        cursor = GetData("SalesRecord").fetchall()
    if len(cursor) > 0:
        result['result'] = True
        result['data'] = cursor
    else:
        result['reason'] = "销售记录为空"
    Event.FireEvent("SendMessage", "sales:record", result, tar=target)


# 退货
def Returns(target, args):
    result = {'type': "returns", 'result': False}
    record_id = 0
    item_id = 0
    if isinstance(args, tuple):
        record_id = args[0]
        item_id = args[1]
    else:
        record_id = args
    # print("退货数据:{0}".format(args))
    now_data = GetData("SalesRecord", record_id).fetchall()[0]
    # print("原数据:{0}".format(now_data))
    goods_list = now_data['goods'].split(';')
    record = []
    for goods in goods_list:
        new_str = goods
        goods_id = goods.split(',')[0]
        if goods_id == item_id or item_id == 0:
            index = findSubStrIndex(',', new_str, 4) + 1
            new_str = sub(new_str, index, 1)
        record.append(new_str)
    record_str = ";".join(record)
    result['result'], result['reason'] = Event.FireEvent("UpData", "SalesRecord", 'goods', record_str, record_id)
    if result['result']:
        result['data'] = GetData("SalesRecord", record_id).fetchall()[0]
    Event.FireEvent("SendMessage", "sales:returns", result, tar=target)


# 找字符串substr在str中第time次出现的位置
def findSubStrIndex(substr, str, time):
    times = str.count(substr)
    if (times == 0) or (times < time):
        pass
    else:
        i = 0
        index = -1
        while i < time:
            index = str.find(substr, index + 1)
            i += 1
        return index


# 替换字符串string中指定位置p的字符为c
def sub(string,p,c):
    new = []
    for s in string:
        new.append(s)
    new[p] = c
    return ''.join('%s' % id for id in new)


# 获取员工数据
def GetStaffData(target, args):
    result = {'type': "get", 'result': False}
    # print("获取员工数据：{0}".format(args))
    if args is not None:
        cursor = GetData("Staff", args).fetchall()
    else:
        cursor = GetData("Staff").fetchall()
    if len(cursor) > 0:
        result['result'] = True
        result['data'] = cursor
    else:
        result['reason'] = "会员为空"
    Event.FireEvent("SendMessage", "staff:data", result, tar=target)


# 添加员工
def AddStaff(target, data):
    result = {'type': "add", 'result': False}
    # print("添加员工数据:{0}".format(data))
    if data is not None:
        join = "%s,%s,%s" % (datetime.datetime.now().year, datetime.datetime.now().month, datetime.datetime.now().day)
        result['result'], result['reason'] = Event.FireEvent("AddData", "Staff", data['Id'], data['Name'], data['Password'], data['Power'], data['Tel'], data['Gender'], data['Birth'], join)
    else:
        result['result'] = False
        result['reason'] = "添加数据为空！"
    if result['result']:
        result['data'] = GetData("Staff", data['Id']).fetchall()[0]
    Event.FireEvent("SendMessage", "staff:add", result, tar=target)


# 删除会员
def DeleteStaff(target, data):
    result = {'type': "delete", 'result': False}
    # print("删除员工数据:{0}".format(data))
    result['result'], result['reason'] = DelData("Staff", data)
    if result['result'] is True:
        result['id'] = data
    Event.FireEvent("SendMessage", "staff:delete", result, tar=target)


# 修改会员
def UpdateStaff(target, data):
    result = {'type': "update", 'result': False}
    # print("修改员工数据:{0}".format(data))
    now_data = GetData("Staff", data['Id'])
    head_info = now_data.description  # 表头
    original = now_data.fetchall()[0]  # 原数据
    # print("原数据:{0}".format(original))
    for field in head_info:
        if original[field[0]] == data[field[0].title()] or field[0] == "enter":
            continue
        result['result'], result['reason'] = Event.FireEvent("UpData", "Staff", field[0], data[field[0].title()], str(data['Id']))
    if result['result']:
        result['data'] = GetData("Staff", data['Id']).fetchall()[0]
    Event.FireEvent("SendMessage", "staff:update", result, tar=target)