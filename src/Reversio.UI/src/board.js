export class Board {
    constructor() {
        this.element = this.createElement();
    }

    createElement() {
      let html = '<table class="board board-white-player">';
      for(let i = 1; i <= 8; i++) {
          html += '<tr>'
          for(let j = 1; j <= 8; j++) {
              let discClass = 'disc-none';
              if((i == 4 && j == 4) || (i == 5 && j == 5)) {
                  discClass = 'disc-white';
              } else if((i == 4 && j == 5) || (i == 5 && j == 4)) {
                  discClass = 'disc-black';
              }
              html += `<td data-row="${i}" data-col="${j}"><span class="disc ${discClass}"></span></td>`
          }
          html += '</tr>'
      }
      html += '</table>';
      return $(html);
    }

    appendToElement(elem) {
        elem.append(this.element);
    }
}