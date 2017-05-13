import MySQLdb
import json
import httplib2
import time
import urllib
cnxn = MySQLdb.connect(host='114.55.175.99',user='autyan',passwd='juli#406',db='ks_dust_platform',port=3306,charset = "utf8")
cursor = cnxn.cursor()
urlstr = 'https://api.seniverse.com/v3/weather/daily.json?key=rpvxjeffnzncbdks&location=shanghai&language=zh-Hans&unit=c&start=1&days=1'
h = httplib2.Http('.cache')
try:
        response, content = h.request(urlstr, 'GET', headers={'Content-Type':'application/x-www-form-urlencoded'})
        weatherJson = json.loads(content)
        nowDate = time.strftime('%Y-%m-%d')
        daily = weatherJson[u'results'][0][u'daily'][0]
        cursor.execute("""INSERT INTO dayweathers (Date,DayText,DayCode,NightText,NightCode,TemperatureHigh,TemperatureLow,WindDirection,
                WindDirectionDegree,WindSpeed,WindScale) VALUES (%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s)""",
                (nowDate, daily[u'text_day'],daily[u'code_day'],daily[u'text_night'],daily[u'code_night'],
                daily[u'high'],daily[u'low'],daily[u'wind_direction'],daily[u'wind_direction_degree'],daily[u'wind_speed'],daily[u'wind_scale']))
        cnxn.commit()
        print(time.strftime('ExecuteTime: %Y-%m-%d %H:%M:%S'))
except Exception as error:
        print(error)
