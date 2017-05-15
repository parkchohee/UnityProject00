var restify = require('restify');
var plugins = require('restify-plugins');
var mysql = require('mysql');
var config = require('./config.json');

var pool = mysql.createPool(config.dbinfo);


const server = restify.createServer({
  name: 'myapp',
  version: '1.0.0'
});

const session = require('express-session');
server.use(session({
  secret:'test',
  saveUninitialized:  true,
  resave : true
}));

server.use(plugins.acceptParser(server.acceptable));
server.use(plugins.queryParser());
server.use(plugins.bodyParser());

// 로그인
server.post('/Login', function(req, res, next) {
  var info = req.body;

  pool.getConnection(function(err,connection) {

    var query = connection.query("select * from Users", function (err, rows) {
      if(err) {
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        try {
          outer : do
           {
            var id;
            for(var key in rows)
             {
              if(rows[key].UserId == info["ID"])
              {
                id = info["ID"];
                if(rows[key].Password != info["PW"])
                {
                  err = new Error('WRONG_PASSWORD');
                  break outer;
                }
              }
            }

            if(!id) {
              err = new Error('WRONG_ID');
              break outer;
            }

            // session setting
            session.loginId = id;

            res.send(200);
            console.log("login성공");

          } while(false)

          if(err) {
            res.status(err.status || 500);
            res.send(err.message);
          }

        } catch(err) {
          res.status(err.status || 500);
          res.send(err.message);
        }
      }
      connection.release();
    });
  });

  return next();
});

// 회원가입
server.post('/SignUp', function(req, res, next) {
  var info = req.body;

  pool.getConnection(function(err,connection){

    var query = connection.query("select * from Users", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        try{
          outer : do {
            for(var key in rows)
            {
              if(rows[key].UserId == info["ID"] || info["ID"].length < 2 || !info["ID"])
              {
                err = new Error('WRONG_ID');
                break outer;
              }
            }

            connection.query('insert into Users (UserId, Password) values ("'
             + info["ID"] + '","' + info["PW"] + '")', function(err, rows) {
              console.log("insert into users");
            });

            res.send(200);
          } while(false)

          if(err) {
            res.status(err.status || 500);
            res.send(err.message);
          }

        } catch(err) {
          res.status(err.status || 500);
          res.send(err.message);
        }
      }
      connection.release();
    });
  });

  return next();
});

// 로그인하고, select창에서 slot을 맨처음 init해주는 역할
/*server.get('/SlotInit', function(req, res, next) {
  pool.getConnection(function(err,connection){
    var query = connection.query("select * from Slots join CharacterInfo on Slots.CharacterNum = CharacterInfo.CharacterNum where UserId = ", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });
    return next();
});*/
server.post('/SlotInit', function(req, res, next) {
  var info = req.body;

  pool.getConnection(function(err,connection){

    var query = connection.query("select * from Slots join CharacterInfo on Slots.CharacterNum = CharacterInfo.CharacterNum where Slots.UserId = '" + info["USER_ID"] + "'", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
          res.send(rows);
      }
      connection.release();
    });
  });

  return next();
});
// 캐릭터 생성할때 모든캐릭터 정보 전달해주는 역할 - 이걸로 모델 만들어줌
server.get('/AllCharacterJobs', function(req, res, next) {
  pool.getConnection(function(err,connection){
    var query = connection.query("select * from Job", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });
    return next();
});

// 캐릭터 생성할때 모든캐릭터 스킬정보 전달해주는 역할
server.get('/AllCharacterSkills', function(req, res, next) {
  pool.getConnection(function(err,connection){
    var query = connection.query("select * from Skill", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });
    return next();
});

// 캐릭터 생성함.
server.post('/CharacterCreate', function(req, res, next) {
  var info = req.body;

  pool.getConnection(function(err,connection){

    var query = connection.query("select * from CharacterInfo", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        try{
          outer : do {
            for(var key in rows)
            {
              if(rows[key].Name == info["NAME"] || info["NAME"].length < 2 || !info["NAME"])
              {
                err = new Error('WRONG_NAME');
                break outer;
              }
            }

            connection.query('insert into CharacterInfo (Name, Job) values("' + info["NAME"] + '",' + info["JOB"] + ")", function(err, rows) {
              console.log("insert into CharacterInfos");
            });

            connection.query('select * from CharacterInfo where Name ="' + info["NAME"] + '"', function(err, rows) {
              connection.query('insert into Slots (UserId, SlotNum, CharacterNum) values("' + info["LOGIN_ID"] + '",'+ info["SLOT_NUM"]+',' + rows[0].CharacterNum + ')', function(err, rows) {
                console.log("insert into Slots" + rows[0].CharacterNum);
              });
              connection.query('insert into PlayerSpawnPoint (CharacterNum) values(' + rows[0].CharacterNum + ')', function(err, rows) {
                console.log("insert into PlayerSpawnPoint" + rows[0].CharacterNum);
              });

            });

            res.send(200);
          } while(false)

          if(err) {
            res.status(err.status || 500);
            res.send(err.message);
          }

        } catch(err) {
          res.status(err.status || 500);
          res.send(err.message);
        }
      }
      connection.release();
    });
  });

  return next();
});

// 캐릭터 넘버 받아서 플레이어 정보 리턴..
server.post('/CharacterInfoByCharacterNum', function(req, res, next) {
  var info = req.body;
  //info["CHARACTER_NUM"]
  pool.getConnection(function(err,connection){

    var query = connection.query("select * from CharacterInfo Join PlayerSpawnPoint On CharacterInfo.CharacterNum = PlayerSpawnPoint.CharacterNum where CharacterInfo.CharacterNum = " + info["CHARACTER_NUM"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });

  return next();
});

// job id 받아서 특정 job 정보 리턴..
server.post('/JobInfoByJobId', function(req, res, next) {
  var info = req.body;
  //info["CHARACTER_NUM"]
  pool.getConnection(function(err,connection){

    var query = connection.query("select * from Job where JobId = " + info["JOB_ID"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });

  return next();
});

// job id 받아서 특정 직업의 스킬 정보 리턴..
server.post('/SkillInfoByJobId', function(req, res, next) {
  var info = req.body;
  //info["CHARACTER_NUM"]
  pool.getConnection(function(err,connection){

    var query = connection.query("select * from Skill where JobId = " + info["JOB_ID"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });

  return next();
});

// 모든 아이템 리스트를 리턴
server.get('/AllItemList', function(req, res, next) {
  pool.getConnection(function(err,connection){
    var query = connection.query("select * from ItemList", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });
    return next();
});

// 캐릭터넘버를 받아서 해당 캐릭터의 아이템 리스트를 리턴
server.post('/ItemListByCharacterNum', function(req, res, next) {
  var info = req.body;
  //info["CHARACTER_NUM"]
  pool.getConnection(function(err,connection){

    var query = connection.query("select * from PlayerItemList join ItemList on PlayerItemList.ItemId = ItemList.ItemId where CharacterNum = " + info["CHARACTER_NUM"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });

  return next();
});

// 모든 몬스터 리스트를 리턴
server.get('/MonsterList', function(req, res, next) {
  pool.getConnection(function(err,connection){
    var query = connection.query("select * from Enemy", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });
    return next();
});

// 캐릭터넘버 받아서 Lev변경
server.post('/Lev', function(req, res, next) {
  var info = req.body;
  pool.getConnection(function(err,connection){

    var query = connection.query("UPDATE CharacterInfo SET Level=" + info["LEV"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        connection.query("UPDATE CharacterInfo SET Exp=" + info["EXP"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
        });
        connection.query("UPDATE CharacterInfo SET MaxExp=" + info["MAX_EXP"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
        });
        connection.query("UPDATE CharacterInfo SET MaxHp=" + info["MAX_HP"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
        });
        connection.query("UPDATE CharacterInfo SET MaxMp=" + info["MAX_MP"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
        });
        connection.query("UPDATE CharacterInfo SET SkillPoint=" + info["SKILL_POINT"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
        });
        res.send(200);
      }
      connection.release();
    });
  });

  return next();
});

// 캐릭터넘버 받아서 Exp변경
server.post('/Exp', function(req, res, next) {
  var info = req.body;
  pool.getConnection(function(err,connection){

    var query = connection.query("UPDATE CharacterInfo SET Exp=" + info["EXP"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(200);
      }
      connection.release();
    });
  });

  return next();
});

// 캐릭터넘버 받아서 Money변경
server.post('/Money', function(req, res, next) {
  var info = req.body;
  pool.getConnection(function(err,connection){

    var query = connection.query("UPDATE CharacterInfo SET Money=" + info["MONEY"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(200);
      }
      connection.release();
    });
  });

  return next();
});

// 캐릭터넘버 받아서 스킬 레벨 변경
server.post('/Skill', function(req, res, next) {
  var info = req.body;
  pool.getConnection(function(err,connection){

    var query = connection.query("UPDATE CharacterInfo SET SkillPoint=" + info["SKILL_POINT"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {

        connection.query("UPDATE CharacterInfo SET SkillLev1=" + info["SKILL_LEV_1"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
        });
        connection.query("UPDATE CharacterInfo SET SkillLev2=" + info["SKILL_LEV_2"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
        });
        connection.query("UPDATE CharacterInfo SET SkillLev3=" + info["SKILL_LEV_3"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
        });

        res.send(200);
      }
      connection.release();
    });
  });

  return next();
});

// 캐릭터넘버 받아서 아이템 추가
server.post('/AddNewItem', function(req, res, next) {
  var info = req.body;
  pool.getConnection(function(err,connection){
    var query = connection.query("Insert into PlayerItemList (CharacterNum, ItemId, ItemSlotNum, ItemCount) values ("+info["CHARACTER_NUM"]+","
                                      + info["ITEM_ID"] +"," + info["ITEM_SLOT_NUM"] + "," + info["ITEM_COUNT"] + ")", function (err, rows) {
        if(err){
          res.status(err.status || 500);
          res.send(err.message);
        } else {
          res.send(200);
        }
        connection.release();
      });
  });

  return next();
});

// 캐릭터넘버 받아서 Money변경
server.post('/UpdateItem', function(req, res, next) {
  var info = req.body;
  pool.getConnection(function(err,connection){

    var query = connection.query("UPDATE PlayerItemList SET ItemCount=" + info["ITEM_COUNT"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"] + " && ItemID =" + info["ITEM_ID"], function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(200);
      }
      connection.release();
    });
  });

  return next();
});

// 게임 종료할때 저장하는 데이터
server.post('/Exit', function(req, res, next) {
  var info = req.body;
  pool.getConnection(function(err,connection){
    connection.query("UPDATE CharacterInfo SET Exp=" + info["EXP"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
    });
    connection.query("UPDATE CharacterInfo SET Hp=" + info["HP"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
    });
    connection.query("UPDATE CharacterInfo SET Mp=" + info["MP"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
    });
    connection.query("UPDATE PlayerSpawnPoint SET X=" + info["X"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
    });
    connection.query("UPDATE PlayerSpawnPoint SET Y=" + info["Y"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
    });
    connection.query("UPDATE PlayerSpawnPoint SET Z=" + info["Z"] + " WHERE CharacterNum=" + info["CHARACTER_NUM"], function(err, rows) {
    });
  });

  return next();
});






server.get('/DBtest', function(req, res, next) {

  pool.getConnection(function(err,connection){
    var query = connection.query("select * from Users", function (err, rows) {
      if(err){
        res.status(err.status || 500);
        res.send(err.message);
      } else {
        res.send(rows);
      }
      connection.release();
    });
  });

  return next();
});



server.listen(80, function () {
  console.log('%s listening at %s', server.name, server.url);
});
