(function ($) {
    var scope = this;
    var pageSize = 10;
    var pageNumber = 1;
    var sortColumn = "";
    var sortOrder = "";
    var searchArray = [];

    var selections = [];
    var hiddenArray = [];
    var $checkMoreTableElements = [];

    $.fn.contextMenu = function (settings) {

        return this.each(function () {
            
            // Open context menu
            $(this).on("contextmenu", function (e) {
                //alert("AAA");
                // return native menu if pressing control
                if (e.ctrlKey) return;

                //open menu
                var $menu = $(settings.menuSelector)
                    .data("invokedOn", $(e.target))
                    .show()
                    .css({
                        position: "absolute",
                        //right: getMenuPosition(e.clientX, 'width'),
                        left: getMenuPosition(e.clientX, 'width', 'scrollLeft'),
                        top: getMenuPosition(e.clientY, 'height', 'scrollTop')
                        //top: getMenuPosition(e.pageY, 'height') - $(settings.menuSelector)['height']()
                    })
                    .off('click')
                    .on('click', 'a', function (e) {
                        $menu.hide();
                        var ids = getIdSelections();
                        console.log("Id sel: " + ids);
                        var $invokedOn = $menu.data("invokedOn");
                        var $selectedMenu = $(e.target);

                        settings.menuSelected.call(this, $invokedOn, $selectedMenu);
                    });

                return false;
            });

            //make sure menu closes on any click
            $('body').click(function () {
                $(settings.menuSelector).hide();
            });
        });

        //function getMenuPosition(mouse, direction) {
        //    var win = $(window)[direction]();
        //    var page = $(window)[direction]();
        //    var menu = $(settings.menuSelector)[direction]();

        //    if (direction == 'width') {
        //        console.log("direction:", direction);
        //        console.log("mouse", mouse);
        //        console.log("menu", Math.round(menu));
        //        console.log("page", page);
        //        console.log("window", win);
        //    }

        //    // opening menu would pass the side of the page
        //    if (mouse + menu > page && menu < mouse) {
        //        return mouse - menu;
        //    }
            
        //    return mouse;
        //}    

        function getMenuPosition(mouse, direction, scrollDir) {
            var win = $(window)[direction](),
                scroll = $(window)[scrollDir](),
                menu = $(settings.menuSelector)[direction](),
                position = mouse + scroll;

            if (direction == 'height') {
                console.log("direction:", direction);
                console.log("mouse", mouse);
                console.log("menu", Math.round(menu));
                
                console.log("window", win);
            }

            // opening menu would pass the side of the page
            if (mouse + menu > win && menu < mouse)
                position -= menu;

            return position;
        }

    };

    $.fn.configTable = function (option) {
        var $this = $(this);

        $this.bootstrapTable({
            url: option,
            queryParams: function (p) {
                var params = {
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    searchTerms: searchArray.toString(),
                    sortColumn: sortColumn,
                    sortOrder: p.order,
                    offset: 0,
                    limit: 10
                };

                return params;
            }
            ,
            formatLoadingMessage: function () {
                return '<button class="btn btn-default" style="margin-top:10px;padding:10px;"> <img src="images/ui-anim_basic_16x16.gif"/> Sacekajte...</button>';
            },
            classes: 'table-no-bordered table-sm table-hover'
            //,
            //disableControlWhenSearch: true
            //,
            //searchOnEnterKey: true
           

            
        });

        $('span.caret').css('display', 'none');

    };

    $.fn.addFilter = function (name,value) {
        if (searchArray.length > 0) {
            for (var i = 0; i < searchArray.length ; i++) {
                //console.log(' arr: ' + searchArray[i]);
                if (searchArray[i].includes(name)) {
                    //console.log('name: ' + name + ' arr: ' + searchArray[i]);
                    searchArray.splice(i, 1);
                }

            }
        }

        searchArray.push(name + ":" + value);
        //$('#' + name).val(value);
        $(name).val(value);
    };

    $.fn.removeFilter = function (name, value) {
      
        for (var i = 0; i < searchArray.length+1; i++) {
            //console.log(' arr: ' + searchArray[i]);
            if (searchArray[i].includes(name)) {
                //console.log('name: ' + name + ' arr: ' + searchArray[i]);
                searchArray.splice(i, 1);
            }
        }
    };

    $.fn.hideColTable = function () {
        var $this = $(this);
         //var visibleColumns = $table.bootstrapTable('getVisibleColumns');
        var hiddenColumns = $this.bootstrapTable('getHiddenColumns');
        for (var i = 0; i < hiddenColumns.length; i++) {
            //console.log(hiddenColumns[i].field);
            var colId = hiddenColumns[i].field;
            hiddenArray.push(colId);
            $('#' + colId).parent().parent().hide();
        }
    };

    $.fn.checkOptionsTable = function ($optionsTableElements) {
        var $this = $(this);

        $this.on('check.bs.table uncheck.bs.table ' +
            'check-all.bs.table uncheck-all.bs.table', function () {
                selections = getIdSelections();
                

                if (selections.length === 1) {
                    for (var i = 0; i < $optionsTableElements.length; i++) {
                        $optionsTableElements[i].prop('disabled', false);
                    }  
                    for (var k = 0; k < $checkMoreTableElements.length; k++) {
                        $checkMoreTableElements[k].prop('disabled', false);
                    }
                } else {
                    if (selections.length === 0) {
                        for (var j = 0; j < $optionsTableElements.length; j++) {
                            $optionsTableElements[j].prop('disabled', true);
                        }
                        for (var k = 0; k < $checkMoreTableElements.length; k++) {
                            $checkMoreTableElements[k].prop('disabled', true);
                        }
                    } else {
                        for (var j = 0; j < $optionsTableElements.length; j++) {
                            $optionsTableElements[j].prop('disabled', true);
                        }
                        for (var m = 0; m < $checkMoreTableElements.length; m++) {
                            $checkMoreTableElements[m].prop('disabled', false);
                        }
                    }     
                }

            });
    };

    $.fn.checkMoreOptionsTable = function ($optionsTableElements) {
        $checkMoreTableElements = $optionsTableElements;       
    };

    $.fn.optionsTable = function () {
        var $this = $(this);

        $this.on('page-change.bs.table', function (e, number, size) {
            pageNumber = number;
            pageSize = size;

            $table.bootstrapTable('refresh', { silent: true }); 
        });

        $this.on('sort.bs.table', function (order, name) {
            //console.log("Name: " + name);
            sortColumn = name;

        });

        //$("#table select").change(function () {

        //    addSearchTerms($table);

        //});

        //$("#table input").keypress(function (event) {
        //    //alert("aaa" + event.which);
        //    //addSearchTerms($table);
        //    if (event.which === 13) {
        //        //alert("aaa");
        //        event.preventDefault();
        //        addSearchTerms($table);
        //    }

        //});

        $('#toolbar').find('select').change(function () {
            $('#table').bootstrapTable('destroy').bootstrapTable({
                exportDataType: $(this).val()
            });
        });


        $this.on('column-switch.bs.table', function (e, field, checked) {
            //console.log('SHOW ' + field + " --- " + checked);
            e.preventDefault();
           

            if (!checked)
                hiddenArray.push(field);

            for (var i = 0; i < hiddenArray.length; i++) {
                //console.log(hiddenArray[i]);
                if (checked && field == hiddenArray[i]) {
                    //console.log("vracen: " + field + " --- " + hiddenArray[i]);
                    hiddenArray.splice(i, 1);
                    $('#' + hiddenArray[i]).parent().parent().show();
                }

                //console.log("sakriven: " + field + " --- " + hiddenArray[i]);
                $('#' + hiddenArray[i]).parent().parent().hide();
                
            }
        });
    };

    $.fn.refreshTable = function () {
        $table.bootstrapTable('refresh');
    };


    $.fn.detailsTable = function (option) {
        var $this = $(this);
        $this.on('expand-row.bs.table', function (e, index, row, $detail) {
            $detail.html('Učitavanje detalja, sačekajte...');
            //alert(row.Id);
            $.ajax({
                url: option,
                type: 'get',

                data: {
                    id: row.Id
                },
                success: function (data) {
                    $detail.html(data);
                },
                error: function () {
                    alert('Ajax error!');
                }
            });
        });
    };

    $.fn.clickOptions = function (option, isNewWindow = true, selectedList=false) {
        var $this = $(this);

        $this.click(function () {
            var ids = getIdSelections();
            if (ids.length === 0) openInNewTabWinBrowser(option);

            if (isNewWindow)
                if (selectedList) {
                    var idStr = "";
                    for (var i = 0; i < ids.length; i++) {
                        idStr += ids[i].Id + ",";
                    }
                    openInNewTabWinBrowser(option + idStr.substring(0, idStr.length - 1));
                }
                else {
                    openInNewTabWinBrowser(option + ids[0].Id);
                }
               
            else
                location.href = option + ids[0].Id;
            
        });
    };

    $.fn.clickButtonWithoutParams = function (option, isNewWindow = true) {
        var $this = $(this);

        $this.click(function () {
           
            if (isNewWindow)
                openInNewTabWinBrowser(option);
            else
                location.href = option;

        });
    };

    

    $.fn.clickPostOptions = function (option) {
        var $this = $(this);

        $this.click(function () {
            var ids = getIdSelections();

            var postData = { id: ids[0].Id };
            

            $.ajax({
                url: option,
                type: 'POST',
                dataType: "json",
                traditional: true,
                data: JSON.stringify(postData)
            });
        });
    };

    $.fn.clickPostOptionsMessage = function (option, message) {
        var $this = $(this);

        $this.click(function () {
            var ids = getIdSelections();

            var postData = { id: ids[0].Id };
            

            $.ajax({
                url: option,
                type: 'POST',
                dataType: "json",
                traditional: true,
                data: JSON.stringify(postData),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    alert(message);
                    location.reload();
                },
                error: function (xhr, textStatus, error) {

                    console.log(xhr.statusText);
                    console.log(textStatus);
                    console.log(error);

                }
            });
        });
    };

    $.fn.dropDownChoice = function (option, message, moreSel = false) {
        var $this = $(this);

        $this.click(function () {
            var ids = getIdSelections();
            var izabraniId;
            if(moreSel)
                izabraniId = 0; 
            else
                izabraniId = $this.attr('id');
            //console.log(izabraniId);
            var postData = { Id: izabraniId, SelectedValues: ids };

            $.ajax({
                url: option,
                type: 'POST',
                dataType: "json",
                traditional: true,
                data: JSON.stringify(postData),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (moreSel) {
                        openInNewTabWinBrowser(option + "?id=" +data.ids);
                    } else {
                        alert(message);
                        location.reload();
                    }
                },
                error: function (xhr, textStatus, error) {
                    
                    console.log(xhr.statusText);
                    console.log(textStatus);
                    console.log(error);
                    
                }
                });
        });
    };

    $.fn.dropDownChoiceWithId = function (izabraniId,option, message) {
        //var $this = $(this);

        var ids = getIdSelections();
        
        var postData = { Id: izabraniId, SelectedValues: ids };

            $.ajax({
                url: option,
                type: 'POST',
                dataType: "json",
                traditional: true,
                data: JSON.stringify(postData),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    alert(message);
                    location.reload();
                },
                error: function (xhr, textStatus, error) {

                    console.log(xhr.statusText);
                    console.log(textStatus);
                    console.log(error);

                }
                });
    };
    

    scope.dateFormat = function (value) {
        return (value === null) ? "" : moment(value).format('DD/MM/YYYY');
    };

    scope.formatYesNo = function (value) {
        return value === 0 ? '' : '<i class="glyphicon glyphicon-ok"></i>';
    };

    scope.formatBooleanYesNo = function (value) {
        return value === false ? '' : '<i class="glyphicon glyphicon-ok"></i>';
    };

    scope.timeFormat = function (value) {
        return (value === null) ? '' : moment(value).format('HH:mm:ss');
    };

    scope.intFormat = function (value) {
        return (value).toLocaleString('de-DE'); 
        //var n = value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ";");
        //n = n.replace(".", ",");
        //return n = n.replace(";", ".");
    };

    function openInNewTabWinBrowser(url) {
        var win = window.open(url, '_blank');
        win.focus();
    }

    function getIdSelections() {
        return $.map($table.bootstrapTable('getSelections'), function (row) {
            var paramsSelection = { Id: row.Id };
            return paramsSelection;
        });
    }

    scope.sumFormatter = function (data) {

        var field = this.field;
        if (data.length === 0) {
            return "Σ: 0<br\><br\><br\>P: 0";
        }
        else {
            return "Σ: " + intFormat(data.reduce(function (sum, row) {
                return Math.round((sum + (+row[field])) * 100) / 100;
            }, 0)) + "<br\><br\><br\>P: " + intFormat(Math.round((data.reduce(function (sum, row) {
                return (Math.round((sum + (+row[field])) * 100) / 100);
            }, 0) / data.length) * 100) / 100);
        }
    };

    scope.totalFormatter = function (data) {
        return data.length;
    };

    scope.avgFormatter = function (data) {

        var field = this.field;
        return data.reduce(function (sum, row) {
            return (Math.round((sum + (+row[field])) * 100) / 100);
        }, 0) / data.length;
    };
    

    //function addSearchTerms(table) {
    //    var searchText = "";
    //    var searchColumn = "";

    //    searchArray = [];

    //    pageNumber = 1;
    //    pageSize = 10;

    //    //alert("AAA");

    //    $("table#table :input").each(function () {

    //        //alert("AAA 1");
    //        var input = $(this);


    //        searchText = input.val();
    //        searchColumn = 'Sasija';
    //            //input.attr('id');

    //        //alert(searchText + " ---- " + searchColumn);
    //        if (typeof searchColumn !== 'undefined') //&& ((searchColumn == 'KontaktNaziv') || (searchColumn == 'Adresa')
    //        {
    //            //alert(input.val() + " ---- " + input.attr('id'));

    //            if (input.val() == "on") {
    //                searchText = input.is(":checked");
    //            }

    //            //searchConcatenate += searchColumn + ":" + searchText + ",";
    //            if (searchText != "")
    //                searchArray.push(searchColumn + ":" + searchText);
    //            //searchT[searchColumn] = searchText;
                
    //        }
    //    });

    //    table.bootstrapTable('refresh');
    //}

})(jQuery);