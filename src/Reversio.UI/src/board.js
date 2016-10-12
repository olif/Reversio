export class Board {
    constructor(gamestate) {
        this.element = this.createElement(gamestate);
    }

    createElement(gameState) {
        let html = `
            <table class="board ${gameState.discOfNextMove.color == -1 ? 'board-black-player' : 'board-white-player'}">

            ${gameState.currentState.reduce((rowAcc, row) => {
                  return rowAcc += 
                  `<tr>

                    ${row.reduce((colAcc, col) => {
                        return colAcc += 
                        `<td>
                            <span class="disc ${this.getDiscClass(col)}"></span>
                        </td>`
                    }, '')}

                    </tr>`
            }, '')}
            </table>
        `;

        let element = $(html);
        $('body').on('click', '.board', (event) => {
            console.log('click');
        })
        return $(html);
    }

    getDiscClass(nr) {
        if(nr == -1) return 'disc-black';
        if(nr == 0) return 'disc-none';
        if(nr == 1) return 'disc-white';
    }

    appendToElement(elem) {
        console.log(elem);
        elem.append(this.element);
    }
}