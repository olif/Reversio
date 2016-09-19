'use strict';

var reversio = function () {

    var defaultBoard = [[0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 1, -1, 0, 0, 0], [0, 0, 0, -1, 1, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0]];

    var obj = {};

    function getDiscType(nr) {
        if (nr === -1) return 'black';else if (nr === 0) return '';else return 'white';
    }

    var makeMove = function makeMove(x, y) {
        console.log(x + y);
    };

    var createBoard = function createBoard() {
        var html = '';
        html += '<table class="board">';
        for (var i = 0; i < defaultBoard.length; i++) {
            html += '<tr>';
            for (var j = 0; j < defaultBoard[i].length; j++) {
                var disc = getDiscType(defaultBoard[i][j]);
                html += '<td data-x="' + i + '" data-y="' + j + '"><span class="' + disc + '"></span></td>';
            }
            html += '</tr>';
        }
        html += '</table>';
        obj = $(html);

        obj.find('td').on('click', function () {
            var x = $(this).data('x');
            var y = $(this).data('y');
            makeMove(x, y);
        });

        return obj;
    };

    return {
        createBoard: createBoard
    };
}();