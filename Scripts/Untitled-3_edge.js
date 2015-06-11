
/**
 * Adobe Edge: symbol definitions
 */
(function ($, Edge, compId) {
    //images folder
    var im = '/Content/images/';

    var fonts = {};


    var resources = [
    ];
    var img_Text_Dom = new Array();
    //var img_Text_states = new Array();
    var img_Text_states = "";
    var img_Text_states_total;
    var img_Text_timeline=new Array();
    img_Text_Dom.push({
        id: 'index_paibing',
        type: 'image',
        fill: ["rgba(0,0,0,0)", im + "index_paibing.png", '0px', '0px', '98%', '98%']
    });

    //img_Text_states.push(
    //   {
    //       "${_Stage}": [["gradient", "background-image", [270, [["rgba(255,255,255,0.00)", 0], ["rgba(255,255,255,0.00)", 100]]]], ["style", "width", "98%"]]
    //   });
    //img_Text_states.push(
    //   {  "${_index_paibing}": [["style", "top","19px"], ["style", "background-size", [98, 98], { valueTemplate: "@@0@@% @@1@@%" }],["style", "overflow", "auto"], ["style", "height", index_paibingHeight], ["style", "opacity", "0.0390625"],  ["style", "left", '-17%'],  ["style", "width", "98%"]  ],
    //   });
    
    
    img_Text_timeline.push(
    { id: "eid5", tween: ["gradient", "${_Stage}", "background-image", [270, [['rgba(255,255,255,0.00)', 0], ['rgba(255,255,255,0.00)', 100]]], { fromValue: [270, [['rgba(255,255,255,0.00)', 0], ['rgba(255,255,255,0.00)', 100]]] }], position: 0, duration: 0 }
  //{ id: "eid13", tween: ["style", "${_index_paibing}", "top", '19px', { fromValue: '19px' }], position: 0, duration: 0, easing: "easeInQuad" },
  //   { id: "eid15", tween: ["style", "${_index_paibing}", "opacity", '0.1796879991889', { fromValue: '0.039062999188899994' }], position: 0, duration: 250, easing: "easeInQuad" },
  //   { id: "eid16", tween: ["style", "${_index_paibing}", "opacity", '0.48437550663948', { fromValue: '0.1796880066394806' }], position: 250, duration: 250, easing: "easeInQuad" },
  //   { id: "eid17", tween: ["style", "${_index_paibing}", "opacity", '0.67968851327896', { fromValue: '0.4843760132789612' }], position: 500, duration: 250, easing: "easeInQuad" },
  //   { id: "eid18", tween: ["style", "${_index_paibing}", "opacity", '0.87500149011612', { fromValue: '0.6796889901161194' }], position: 750, duration: 250, easing: "easeInQuad" },
  // { id: "eid19", tween: ["style", "${_index_paibing}", "opacity", '1', { fromValue: '0.8750010132789612' }], position: 1000, duration: 250, easing: "easeInQuad" }
        );
    img_Text_states = '"${_Stage}": [["gradient", "background-image", [270, [["rgba(255,255,255,0.00)", 0], ["rgba(255,255,255,0.00)", 100]]]]],';
    //img_Text_states = img_Text_states + '"${_index_paibing}": [["style", "top","19px"], ["style", "background-size", [98, 98], { valueTemplate: "@@0@@% @@1@@%" }],["style", "overflow", "auto"], ["style", "height", index_paibingHeight], ["style", "opacity", "0.0390625"],  ["style", "left", " - 17 % "],  ["style", "width", "98%"]],';

    ////获取菜品信息
    //$.getJSON("/Stage/getRandomProduct", { RestaurantId: RestaurantId, peopleCount: peopleCount }, function (msg) {
    //    $.each(msg, function (idx, item) {

    //        idx = ++idx;
    //        arr[idx] = item.ProductName;
    //        ids[idx] = item.Id;
    //        prices[idx] = item.Price;
    //        populars[idx] = item.Popular;
    //        hots[idx] = item.Hot;
    //        ADescriptions[idx] = item.ADescription;
    //       // $("#Stage_Text" + idx).html(item.ProductName);
    //       // $("#Stage_Text" + idx).attr("ids", item.Id);

    //    });
    //});

    for (var i = 1; i < parseInt(peopleCount) + 1 ; i++) {
        //创建对象
        //img_Text_Dom.push({
        //    id: 'wujiang' + i,
        //    type: 'image',
        //    tag: 'img',
        //    cursor: ['pointer'],
        //    fill: ["rgba(0,0,0,0)", im + "wujiang" + i + ".png", '0px', '0px', '82px', '82px']
        //}, {
        //    id: 'Text' + i,
        //    type: 'text',
        //    tag: 'p',
        //    text: arr[i],
        //    ids:ids[i],
        //    font: ['Arial, Helvetica, sans-serif', 24, "#e4ff00", "normal", "none", ""]
        //});
        //给对象添加样式及对象位置
        
        //img_Text_states.push(
        //     {
        //         "${_Text1}": [
        //           ["style", "top", '-19%'],
        //           ["style", "font-size", '12px'],
        //           ["style", "left", '-32%'],
        //           ["style", "color", '#e4ff00'],
        //           ["style", "text-align", 'center'],
        //           ["style", "width", '25%']
        //         ],

        //         "${_wujiang1}": [
        //                      ["style", "top", '-90px'],
        //                      ["style", "cursor", 'pointer'],
        //                      ["style", "height", '82px'],
        //                      ["style", "background-size", [82, 82], { valueTemplate: '@@0@@px @@1@@px' }],
        //                      ["style", "left", '-20%'],
        //                      ["style", "width", '82px']
        //         ]
        //     });

        //img_Text_states = img_Text_states + '"${_Text' + i + '}": [["style", "top", "19%"],["style", "font-size", "12px"],["style", "left", "32%"],["style", "color", "#e4ff00"],["style", "text-align", "center"],["style", "width", "25%"]],';
        //if (i + 1 < parseInt(peopleCount) + 1) {
        //    img_Text_states = img_Text_states + '"${_wujiang' + i + '}": [["style", "top", "-90px"],["style", "cursor", "pointer"],["style", "height", "82px"],["style", "background-size", [82, 82], { valueTemplate: "@@0@@px @@1@@px" }],["style", "left", "-20%"],["style", "width", "82px"]],';
        //} else {
        //    img_Text_states = img_Text_states + '"${_wujiang' + i + '}": [["style", "top", "-90px"],["style", "cursor", "pointer"],["style", "height", "82px"],["style", "background-size", [82, 82], { valueTemplate: "@@0@@px @@1@@px" }],["style", "left", "-20%"],["style", "width", "82px"]]';
        //}

       // //添加动画
       // img_Text_timeline.push(
       //{

       //    id: "eid" + i, tween: ["style", "${_wujiang" + i + "}", "left", '174px', { fromValue: '166px' }], position: 1250, duration: 500, easing: "easeInQuad"
       //},
       //{
       //    id: "eid20" + i, tween: ["style", "${_wujiang" + i + "}", "top", '14px', { fromValue: '-90px' }], position: 1250, duration: 500, easing: "easeInOutElastic"
       //},

       //{
       //    id: "eid58" + i, tween: ["style", "${_Text" + i + "}", "font-size", '32px', { fromValue: '32px' }], position: 0, duration: 0, easing: "easeOutElastic"
       //},
       //{ id: "eid60"+i, tween: ["style", "${_Text"+i+"}", "font-size", '12px', { fromValue: '32px' }], position: 1000, duration: 750, easing: "easeOutElastic" }
       // );
      
    }
    var lx = 0, ly = 0, aw = 0;
    var contentHeight = document.documentElement.clientHeight * 0.71 - 98;
    if (contentHeight > document.documentElement.clientWidth) {
       // aw = document.documentElement.clientWidth - 100;
        aw = document.documentElement.clientWidth;
        ly = document.documentElement.clientHeight * 0.21 + (contentHeight - aw) / 2;
    }
    else {
        aw = contentHeight;
        lx = (document.documentElement.clientWidth - contentHeight)/2;
        ly = document.documentElement.clientHeight * 0.21+28;
    }
    calcLocation("Stage", { x: lx, y: ly }, { width: aw, height: aw }, productLength);
   
    //矩形中生成任意几何图形
  
    function calcLocation(id, loc, area, count) {
        var screenObj = $("#" + id);

        screenObj.css("top", loc.y + "px");
        screenObj.css("left", loc.x + "px");
         //screenObj.css("width", area.width + "px");
       // screenObj.css("width", 97+ "%");
       screenObj.css("height", area.height + "px");
      // screenObj.css("background-color", "red");
        var colCount;

        
        for (var sq = 1; sq < 10; sq++) {
            if (sq * sq > count) {
                colCount = new Array();
                for (var sqi = 0; sqi < sq; sqi++) {
                    colCount.push(sq);
                }
                var cidx = 0;
                for (var sqi = count; sqi < sq*sq; sqi++) {
                    colCount[cidx] = colCount[cidx] - 1;
                    cidx = cidx + 1;
                    cidx = cidx % (sq - 1);
                }
                break;
            }
        }
        /*switch (count) {
            case 2:
                colCount = new Array(1, 0, 1);
                break;
            case 3:
                colCount = new Array(1, 0, 2);
                break;
            case 4:
                colCount = new Array(1, 1, 2);
               
                break;
            case 5:
                colCount = new Array(2, 1, 2);
                break;
            case 6:
                colCount = new Array(2, 1, 3);
                break;
            case 7:
                colCount = new Array(1, 3, 3);
                break;
            case 8:
                colCount = new Array(2, 3, 3);
                break;
            case 9:
                colCount = new Array(3, 3, 3);
                break;
            default:
                colCount = new Array(3, 3, 3);
                break;

        }*/
        var maxCount = 1;
        for (var i = 0; i < colCount.length; i++) {
            if (colCount[i] > maxCount)
                maxCount = colCount[i];
        }
        var pading = 2;
        var sWidth = (area.width - pading) / colCount.length;
        var sHeight = (area.height - pading) / maxCount;
        var k = 1; var textTop; var imgi = 1;
        for (var i = 0; i < colCount.length; i++) {
            var cnt = colCount[i];
            var thetop = pading + (maxCount - cnt) / 2 * sHeight;
            for (var j = 0; j < cnt; j++) {
                //创建对象
                img_Text_Dom.push({
                    id: 'wujiang' + k,
                    type: 'image',
                    tag: 'img',
                    cursor: ['pointer'],
                  //  onclick:dialog(arr[k]),
                    fill: ["rgba(0,0,0,0)", im + "wujiang" + imgi + ".png", '0px', '0px', sWidth + "px", sHeight + "px"]
                }, {
                    id: 'Text' + k,
                    type: 'text',
                    tag: 'p',
                    text: arr[k],
                    ids: ids[k],
                    font: ['Arial, Helvetica, sans-serif', 24, "#e4ff00", "normal", "none", ""]
                });
                //给对象添加样式及对象初始位置
                var imgleft = pading + sWidth * i;

                var top = (thetop + sHeight * j + (sHeight - sWidth) / 2);
               
                textTop = top + sHeight-15;
                img_Text_states = img_Text_states + '"${_Text' + k + '}": [["style", "top", "' + top + 'px"],["style", "font-size", "12px"],["style", "overflow", "hidden"],["style", "text-overflow", "ellipsis"],["style", "left","' + imgleft + '+px"],["style", "color", "#e4ff00"],["style", "text-align", "center"],["style", "width", ' + sWidth + ']],';
                if (k + 1 < parseInt(count) + 1) {
                    img_Text_states = img_Text_states + '"${_wujiang' + k + '}": [["style", "top", "-350px"],["style", "background-image", "url(' + im + 'index1.png)"],["style", "cursor", "pointer"],["style", "height", "' + sHeight + 'px"],["style", "background-size", [' + sWidth + ', ' + sHeight + '], { valueTemplate: "@@0@@px @@1@@px" }],["style", "left","100px"],["style", "width", "' + sWidth + 'px"]],';
                } else {
                    img_Text_states = img_Text_states + '"${_wujiang' + k + '}": [["style", "top","-350px"],["style", "background-image", "url(' + im + 'index1.png)"],["style", "cursor", "pointer"],["style", "height", "' + sHeight + 'px"],["style", "background-size", [' + sWidth + ', ' + sHeight + '], { valueTemplate: "@@0@@px @@1@@px" }],["style", "left","100px"],["style", "width", "' + sWidth + 'px"]]';
                }
               
                //var divTag = document.createElement("div");
                //screenObj[0].appendChild(divTag);
                //var thisTag = $(divTag);
                //thisTag.css("top", (thetop + sHeight * j + (sHeight - sWidth) / 2) + "px");
                //thisTag.css("left", pading + sWidth * i + "px");
                //thisTag.css("width", sWidth - pading);
                //thisTag.css("height", sWidth - pading);
                //thisTag.css("background-color", "blue");
                //thisTag.css("position", "absolute");



                //添加动画
                img_Text_timeline.push(
               {

                   id: "eid" + i, tween: ["style", "${_wujiang" + k + "}", "left", '' + imgleft + 'px', { fromValue: '166px' }], position: 1250, duration: 500, easing: "easeInQuad"
               },
               {
                   id: "eid20" + i, tween: ["style", "${_wujiang" + k + "}", "top", '' + top + 'px', { fromValue: '-90px' }], position: 1250, duration: 500, easing: "easeInOutElastic"
               },

               {
                   id: "eid58" + i, tween: ["style", "${_Text" + k + "}", "font-size", '12px', { fromValue: '32px' }], position: 0, duration: 0, easing: "easeOutElastic"
               },
               {
                   id: "eid60" + i, tween: ["style", "${_Text" + k + "}", "font-size", '12px', { fromValue: '32px' }], position: 1250, duration: 500, easing: "easeOutElastic"
               },
               { id: "eid50" + i, tween: ["style", "${_Text" + k + "}", "top", '' + textTop + 'px', { fromValue: '97px' }], position: 0, duration: 1250, easing: "easeOutBounce" }

                );
                imgi++
                if (imgi == 7) {
                    imgi = 1;
                }
                k++;
               
            }
           
        }
    }









    var  content={
        dom: img_Text_Dom,
        symbolInstances: [

        ]
    };
    img_Text_states_total = eval("(" + '{"Base State":{' + img_Text_states + '}}' + ")");
     
    var symbols = {
        "stage": {
            version: "1.5.0",
            minimumCompatibleVersion: "1.5.0",
            build: "1.5.0.217",
            baseState: "Base State",
            initialState: "Base State",
            gpuAccelerate: false,
            resizeInstances: false,
            content:content,
            states: img_Text_states_total,
            //states: {
            //    "Base State": {
            //        "${_Text7}": [
            //           ["style", "top", '-19%'],
            //           ["style", "font-size", '12px'],
            //           ["style", "left", '-32%'],
            //            ["style", "color", '#e4ff00'],
            //             ["style", "text-align", 'center'],
            //           ["style", "width", '25%']
            //        ],
            //        "${_wujiang1}": [
            //           ["style", "top", '-90px'],
            //           ["style", "cursor", 'pointer'],
            //           ["style", "height", '82px'],
            //           ["style", "background-size", [82, 82], { valueTemplate: '@@0@@px @@1@@px' }],
            //           ["style", "left", '-20%'],
            //           ["style", "width", '82px']
            //        ],
            //        "${_wujiang7}": [
            //           ["style", "top", '-16%'],
            //           ["style", "cursor", 'pointer'],
            //           ["style", "height", '86px'],
            //           ["style", "background-size", [82, 82], { valueTemplate: '@@0@@px @@1@@px' }],
            //           ["style", "left", '-29%'],
            //           ["style", "width", '71px']
            //        ],
            //        "${_Text2}": [
            //           ["style", "top", '-2%'],
            //           ["style", "height", '28px'],
            //           ["style", "width", '25%'],
            //           ["style", "left", '-26%'],
            //           ["style", "color", '#e4ff00'],
            //           ["style", "text-align", 'center'],
            //           ["style", "font-size", '24px']
                       
            //        ],
            //        "${_index_paibing}": [
            //           ["style", "top", '19px'],
            //           ["style", "background-size", [98, 98], { valueTemplate: '@@0@@% @@1@@%' }],
            //           ["style", "overflow", 'auto'],
            //       ["style", "height", index_paibingHeight],
            //           ["style", "opacity", '0.0390625'],
            //             ["style", "left", '3%'],
            //           ["style", "width", '98%']
            //        ],
            //        "${_wujiang4}": [
            //           ["style", "top", '-8%'],
            //           ["style", "cursor", 'pointer'],
            //           ["style", "height", '82px'],
            //           ["style", "background-size", [82, 82], { valueTemplate: '@@0@@px @@1@@px' }],
            //           ["style", "left", '-26%'],
            //           ["style", "width", '82px']
            //        ],
            //        "${_Text4}": [
            //          ["style", "top", '-13%'],
            //           ["style", "font-size", '12px'],
            //           ["style", "left", '-25%'],
            //            ["style", "color", '#e4ff00'],
            //             ["style", "text-align", 'center'],
            //            ["style", "width", '25%']
            //        ],
            //        "${_Text1}": [
            //           ["style", "top", '34%'],
            //           ["style", "font-size", '32px'],
            //           ["style", "left", '32%'],
            //            ["style", "color", '#e4ff00'],
            //             ["style", "text-align", 'center'],
            //           ["style", "width", '25%']
            //        ],
            //        "${_Text5}": [
            //          ["style", "top", '-13%'],
            //           ["style", "font-size", '12px'],
            //           ["style", "left", '-32%'],
            //            ["style", "color", '#e4ff00'],
            //             ["style", "text-align", 'center'],
            //           ["style", "width", '25%']
            //        ],
            //        "${_Text3}": [
            //         ["style", "top", '-13%'],
            //           ["style", "font-size", '12px'],
            //           ["style", "left", '-18%'],
            //            ["style", "color", '#e4ff00'],
            //             ["style", "text-align", 'center'],
            //          ["style", "width", '25%']
            //        ],
            //        "${_wujiang2}": [
            //           ["style", "top", '-88px'],
            //           ["style", "cursor", 'pointer'],
            //           ["style", "height", '82px'],
            //           ["style", "background-size", [82, 82], { valueTemplate: '@@0@@px @@1@@px' }],
            //           ["style", "left", '-30%'],
            //           ["style", "width", '82px']
            //        ],
            //        "${_Text6}": [
            //           ["style", "top", '-19%'],
            //           ["style", "font-size", '12px'],
            //           ["style", "left", '-25%'],
            //            ["style", "color", '#e4ff00'],
            //             ["style", "text-align", 'center'],
            //           ["style", "width", '25%']
            //        ],
            //        "${_wujiang5}": [
            //           ["style", "top", '-8%'],
            //           ["style", "cursor", 'pointer'],
            //           ["style", "height", '82px'],
            //           ["style", "background-size", [82, 82], { valueTemplate: '@@0@@px @@1@@px' }],
            //           ["style", "left", '-34%'],
            //           ["style", "width", '82px']
            //        ],
            //        "${_wujiang6}": [
            //           ["style", "top", '-16%'],
            //           ["style", "cursor", 'pointer'],
            //           ["style", "height", '82px'],
            //           ["style", "background-size", [82, 82], { valueTemplate: '@@0@@px @@1@@px' }],
            //           ["style", "left", '-24%'],
            //           ["style", "width", '82px']
            //        ],
            //        "${_Stage}": [
            //          // ["color", "background-color", 'rgba(255,255,255,0.92)'],
            //          // ["style", "overflow", 'hidden'],
            //          // ["style", "height", '400px'],
            //           ["gradient", "background-image", [270, [['rgba(255,255,255,0.00)', 0], ['rgba(255,255,255,0.00)', 100]]]],
            //           ["style", "width", '98%']
            //        ],
            //        "${_wujiang3}": [
            //           ["style", "top", '-8%'],
            //           ["style", "cursor", 'pointer'],
            //           ["style", "height", '82px'],
            //           ["style", "background-size", [82, 82], { valueTemplate: '@@0@@px @@1@@px' }],
            //           ["style", "left", '-18%'],
            //           ["style", "width", '82px']
            //        ]
            //    }
            //},
            timelines: {
                "Default Timeline": {
                    fromState: "Base State",
                    toState: "",
                    duration: 1750,
                    autoPlay: true,
                   timeline:img_Text_timeline,
                    //timeline: [
                    //   { id: "eid35", tween: ["style", "${_wujiang7}", "left", '297px', { fromValue: '281px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid49", tween: ["style", "${_Text3}", "left", '107px', { fromValue: '162px' }], position: 0, duration: 1750 },
                    //  // { id: "eid6", tween: ["color", "${_Stage}", "background-color", 'rgba(255,255,255,0.92)', { animationColorSpace: 'RGB', valueTemplate: undefined, fromValue: 'rgba(255,255,255,0.92)' }], position: 0, duration: 0 },
                    //   { id: "eid32", tween: ["style", "${_wujiang6}", "top", '238px', { fromValue: '-79px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid21", tween: ["style", "${_wujiang2}", "top", '14px', { fromValue: '-88px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid42", tween: ["style", "${_Text7}", "left", '299px', { fromValue: '162px' }], position: 0, duration: 1750 },
                    //   { id: "eid47", tween: ["style", "${_Text4}", "left", '228px', { fromValue: '162px' }], position: 0, duration: 1750 },
                    //   { id: "eid50", tween: ["style", "${_Text3}", "top", '215px', { fromValue: '97px' }], position: 0, duration: 1750, easing: "easeOutBounce" },
                    //   { id: "eid11", tween: ["style", "${_index_paibing}", "left", '95px', { fromValue: '95px' }], position: 0, duration: 0, easing: "easeInQuad" },
                    //   { id: "eid45", tween: ["style", "${_Text5}", "left", '349px', { fromValue: '162px' }], position: 0, duration: 1750 },
                    //   { id: "eid37", tween: ["style", "${_Text2}", "font-size", '12px', { fromValue: '24px' }], position: 0, duration: 1750 },
                    //   { id: "eid40", tween: ["style", "${_Text2}", "top", '98px', { fromValue: '100px' }], position: 0, duration: 1750 },
                    //   { id: "eid24", tween: ["style", "${_wujiang1}", "left", '174px', { fromValue: '166px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid38", tween: ["style", "${_Text2}", "width", '18.19%', { fromValue: '2.37%' }], position: 0, duration: 1750 },
                    //   { id: "eid15", tween: ["style", "${_index_paibing}", "opacity", '0.1796879991889', { fromValue: '0.039062999188899994' }], position: 0, duration: 250, easing: "easeInQuad" },
                    //   { id: "eid16", tween: ["style", "${_index_paibing}", "opacity", '0.48437550663948', { fromValue: '0.1796880066394806' }], position: 250, duration: 250, easing: "easeInQuad" },
                    //   { id: "eid17", tween: ["style", "${_index_paibing}", "opacity", '0.67968851327896', { fromValue: '0.4843760132789612' }], position: 500, duration: 250, easing: "easeInQuad" },
                    //   { id: "eid18", tween: ["style", "${_index_paibing}", "opacity", '0.87500149011612', { fromValue: '0.6796889901161194' }], position: 750, duration: 250, easing: "easeInQuad" },
                    //   { id: "eid19", tween: ["style", "${_index_paibing}", "opacity", '1', { fromValue: '0.8750010132789612' }], position: 1000, duration: 250, easing: "easeInQuad" },
                    //   { id: "eid28", tween: ["style", "${_wujiang4}", "top", '122px', { fromValue: '-87px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid51", tween: ["style", "${_Text2}", "height", '15px', { fromValue: '28px' }], position: 0, duration: 1750 },
                    //   { id: "eid26", tween: ["style", "${_wujiang3}", "top", '126px', { fromValue: '-89px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid34", tween: ["style", "${_wujiang7}", "top", '233px', { fromValue: '-90px' }], position: 1250, duration: 500, easing: "easeInElastic" },
                    //   { id: "eid39", tween: ["style", "${_Text2}", "left", '283px', { fromValue: '186px' }], position: 0, duration: 1750, easing: "easeOutSine" },
                    //   { id: "eid30", tween: ["style", "${_wujiang5}", "top", '128px', { fromValue: '-88px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid5", tween: ["gradient", "${_Stage}", "background-image", [270, [['rgba(255,255,255,0.00)', 0], ['rgba(255,255,255,0.00)', 100]]], { fromValue: [270, [['rgba(255,255,255,0.00)', 0], ['rgba(255,255,255,0.00)', 100]]] }], position: 0, duration: 0 },
                    //   { id: "eid58", tween: ["style", "${_Text1}", "font-size", '32px', { fromValue: '32px' }], position: 0, duration: 0, easing: "easeOutElastic" },
                    //   { id: "eid60", tween: ["style", "${_Text1}", "font-size", '12px', { fromValue: '32px' }], position: 1000, duration: 750, easing: "easeOutElastic" },
                    //   { id: "eid46", tween: ["style", "${_Text5}", "top", '215px', { fromValue: '97px' }], position: 0, duration: 1750, easing: "easeOutBounce" },
                    //   { id: "eid27", tween: ["style", "${_wujiang3}", "left", '107px', { fromValue: '102px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid43", tween: ["style", "${_Text6}", "left", '178px', { fromValue: '162px' }], position: 0, duration: 1750 },
                    //   { id: "eid33", tween: ["style", "${_wujiang6}", "left", '171px', { fromValue: '159px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid29", tween: ["style", "${_wujiang4}", "left", '233px', { fromValue: '227px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid41", tween: ["style", "${_Text7}", "top", '318px', { fromValue: '97px' }], position: 0, duration: 1750, easing: "easeOutBounce" },
                    //   { id: "eid48", tween: ["style", "${_Text4}", "top", '213px', { fromValue: '97px' }], position: 0, duration: 1750, easing: "easeOutBounce" },
                    //   { id: "eid13", tween: ["style", "${_index_paibing}", "top", '19px', { fromValue: '19px' }], position: 0, duration: 0, easing: "easeInQuad" },
                    //   { id: "eid20", tween: ["style", "${_wujiang1}", "top", '14px', { fromValue: '-90px' }], position: 1250, duration: 500, easing: "easeInOutElastic" },
                    //   { id: "eid44", tween: ["style", "${_Text6}", "top", '318px', { fromValue: '97px' }], position: 0, duration: 1750, easing: "easeOutBounce" },
                    //   { id: "eid31", tween: ["style", "${_wujiang5}", "left", '355px', { fromValue: '344px' }], position: 1250, duration: 500, easing: "easeInQuad" },
                    //   { id: "eid22", tween: ["style", "${_wujiang2}", "left", '288px', { fromValue: '277px' }], position: 1250, duration: 500, easing: "easeInQuad" }]
                }
            }
        }
    };


    Edge.registerCompositionDefn(compId, symbols, fonts, resources);

    /**
     * Adobe Edge DOM Ready Event Handler
     */
    $(window).ready(function () {
        Edge.launchComposition(compId);
       
    });
})(jQuery, AdobeEdge, "EDGE-9027059");
