/** Function that allows droppable elements by preventing the default action
 * of rejecting drop
 * 
 * @param {any} event: Event object for the dragover event
 */
function allowDrop(event) {
    event.preventDefault();
}

/** Function that is called when the a given element with the draggable= true attribute
 * gets dragged in the dragstart event
 * 
 * @param {any} event: The object for the dragstart event
 */
function drag(event) {
    //event.dataTransfer.setData("text", event.target.id);
    event.dataTransfer.setData("text", event.target.closest('.participant').id);

}

/** Function that will handle dropping a participant object into the player slots in the tournament
 * bracket
 * 
 * @param {any} event: Drop event object that lets us get the participant and slot information
 */
function drop(event) {
    //Prevent the default action
    event.preventDefault();
    //Get the text data of what we are dropping (participant)
    var data = event.dataTransfer.getData("text");
    //Get the participant element
    var draggedElement = document.getElementById(data);
    //console.log(draggedElement);

    // Check if the drop target is a player slot and if it empty (no child image element)
    if (event.target.classList.contains('player-slot') && event.target.children.length === 0) {
        //Drop the participant in the slot
        event.target.appendChild(draggedElement);
    } else {
        //Alert the user the dropping is not possible!
        alert("This slot is already occupied!");
    }
}

// Increment counter
function incrementCounter(button) {
    var counter = button.parentNode.querySelector('.counter');
    counter.textContent = parseInt(counter.textContent) + 1;
}

// Decrement counter
function decrementCounter(button) {
    var counter = button.parentNode.querySelector('.counter');
    if (parseInt(counter.textContent) > 0) {
        counter.textContent = parseInt(counter.textContent) - 1;
    }
}

/** Function that will find the smallest number that the parameter value is a multiple of
 * Excluding 2 from the possible values returned. 
 * The minimum number of participants is 3, making 4 the smallest possible even multiple
 * @param {int} number: The number we want to find the multiple of
 * @returns {int}: The smallest possible multiple of the parameter from 3 on. 
 */
function findSmallestMultiple(number) {
    //For all the numbers from 2 until the parameter value
    for (var i = 3; i < number; i++) {
        //Check if param is divisible by this number
        if (number % i == 0) {
            return i;
        }

    }
    return number;
}

/** Function will generate the tournament bracket html code to the webpage
 * Uses the number of participants to dynamically generate the bracket and 
 * attaches it as a child to the given html element parameter.
 * 
 * @param {int} participants: The number of participants in the tournament
 * @param {string} elementID: The ID of the tournament bracket div html element to append to
 */
function generateTournamentBracket(participants, elementID) {
    //Get the tournament DIV
    var tournamentDiv = document.getElementById(elementID);

    //Create the bracket div
    var bracketDiv = document.createElement("div");
    bracketDiv.className = "bracket";

    //Switch case to send the number of columns to the respective dynamic function based on the number of participants
    switch (findSmallestMultiple(participants)) {
        case 3:
            //Multiple of 3
            break;
        case 4:
            multipleOfFour(Math.sqrt(participants), participants, bracketDiv);
            break;
        default:
            //Prime number?
    }

    //Append the bracket to the html file
    tournamentDiv.appendChild(bracketDiv);
}


function generateLosersBracket(participants, elementID) {
    //Get the tournament DIV
    var tournamentDiv = document.getElementById(elementID);

    //Create the bracket div
    var bracketDiv = document.createElement("div");
    bracketDiv.className = "bracket";

    //Switch case to send the number of columns to the respective dynamic function based on the number of participants
    switch (findSmallestMultiple(participants)) {
        case 3:
            //Multiple of 3
            multipleOfThree(participants / 3, participants, bracketDiv);
            break;
        case 4:
            multipleOfFour(Math.sqrt(participants), participants, bracketDiv);
            break;
        default:
        //Prime number?
    }

    //Append the bracket to the html file
    tournamentDiv.appendChild(bracketDiv);
}

function multipleOfThree(columns, participants, bracketDiv) {

    /*
    Matches per column:
    15 man:
     4 | 4 | 2 ("3") | 2 | 1 | Winner

    */
    //Calculate the matches per column. First column is sqrt of participants rounded up
    //to next even number 
    var matchesPerColumn = Math.sqrt(participants + 1);

    //For the number of columns in the bracket
    for (var i = 1; i <= columns; i++) {
        //generate the column
        var columnDiv = document.createElement("div");
        columnDiv.className = `column ${i}`;

        //CHANGE
        generateLosersColumns(i, i == 3 ? matchesPerColumn + 1 : matchesPerColumn, columnDiv);

        //If we are on an even column
        if (i % 2 == 0) {
            //Half the next set of matches per column
            matchesPerColumn /= 2;
        }
        
        //Append the column to the bracket
        bracketDiv.appendChild(columnDiv);
    }

    //Generate the Final column in the bracket
    generateWinnerColumn(bracketDiv);

    //Rename line extended to line one
    var lineRename = bracketDiv.querySelector(".column.last .line.extended");
    lineRename.className = "line one";
}

function generateLosersColumns(columnNum, matches, columnDiv) {
    //Variations of matches in columns
    //Single Line Match (generateMatch with isFinals = true)
    //Normal Match
    //Single Slot-Single Line Match

    for (var i = 0; i < matches; i++) {
        var matchDiv = document.createElement("div");
        matchDiv.className = "match";

        //For the first and last columns
        if (columnNum == 1 || columnNum == 5) {
            //Single Line Matches
            generateMatch(matchDiv, true);
            //For the Second Column
        } else if (columnNum == 2) {
            //Upper 2 matches Single Line, lower 2 normal
            //generateMatch(matchDiv, i <= matches / 2);
            generateMatch(matchDiv, i == 0 || i == 1);
            //For the Third Column
        } else if (columnNum == 3) {
            //If we are at the last match for the column
            if (i == matches - 1) {
                //Single Slot-Single Line (No bottom team)
                generateMatch(matchDiv, true);
                //matchDiv.querySelector(".match-bottom.team").remove();
            } else {
                //Otherwise generate normal slots
                generateMatch(matchDiv, false);
            }
            //Normal column formatting (really only column 4)
        } else {
            //Normal Match
            generateMatch(matchDiv, false);
        }

        //Append the match to the column
        columnDiv.appendChild(matchDiv);
    }

}

function generateFinalsMatchBracket(elementID) {
    var tournamentDiv = document.getElementById(elementID);

    //Create the bracket div
    var bracketDiv = document.createElement("div");
    bracketDiv.className = "bracket";

    //Create a 1v1 tournament Bracket

    //generate the column
    var columnDiv = document.createElement("div");
    columnDiv.className = `column 1`;
    generateColumn(1, columnDiv, true);

    //Append the column to the bracket
    bracketDiv.appendChild(columnDiv);
    
    //Generate the Final column in the bracket
    generateWinnerColumn(bracketDiv);

    //Append the bracket to the html file
    tournamentDiv.appendChild(bracketDiv);
}

/** Function will dynamically create a tournament bracket that has a multiple of 4 participants
 * 
 * @param {int} columns: The number of columns that will be generated 
 * @param {int} participants: The number of participants
 * @param {var} bracketDiv: The Div element that represents the bracket we will append to
 */
function multipleOfFour(columns, participants, bracketDiv) {

    //The first round will have half of the number of participants of rounds
    var matchesPerColumn = participants / 2;

    //For each of the columns needed
    for (var i = 1; i <= columns; i++) {
        //generate the column
        var columnDiv = document.createElement("div");
        columnDiv.className = `column ${i}`; 

        generateColumn(matchesPerColumn, columnDiv, i == columns);

        //Half the number of matches for the next round
        matchesPerColumn /= 2;
        //Append the column to the bracket
        bracketDiv.appendChild(columnDiv);
    }

    //Generate the Final column in the bracket
    generateWinnerColumn(bracketDiv);

}

/** Function generates the winner column of the bracket
 * 
 * @param {any} bracketdiv: The bracket div to append the winner column to
 */
function generateWinnerColumn(bracketdiv) {
    //Create the final column for the winner
    var winnerColumnDiv = document.createElement("div");
    winnerColumnDiv.className = "column last";


    var winnerDiv = document.createElement("div");
    winnerDiv.className = "winner";

    //Append the crown image
    var img = document.createElement("img");
    img.src = "https://tournamentbracketimages.blob.core.windows.net/pokken-theme-assets/crown.png"
    img.className = "crown"

    winnerDiv.appendChild(img);

    //Generate the winner slot "match"
    generatePlayerSlot(winnerDiv, "winner-slot");

    //Create line div's for CSS
    var matchLinesAltDiv = document.createElement("div");
    matchLinesAltDiv.className = "match-lines alt";

    var lineExtensionDiv = document.createElement("div");
    //lineExtensionDiv.className = "line extended";
    lineExtensionDiv.className = "line extended";

    //Append Elements 
    matchLinesAltDiv.appendChild(lineExtensionDiv);

    winnerColumnDiv.appendChild(winnerDiv);
    winnerColumnDiv.appendChild(matchLinesAltDiv);

    //append to the bracket
    bracketdiv.appendChild(winnerColumnDiv);
}

/** Function will generate a column of matches 
 * based on the number of matches that will be in the column
 * 
 * @param {int} matches The number of matches in the column
 * @param {Var} columnDiv The Div element of the column that will be generated
 * @param {Boolean} isFinals Boolean value determining if the column generated is a finals match
 *                              (Special case for the finals match when generating column)
 */
function generateColumn(matches, columnDiv, isFinals) {

    /*Last match in the last column does not inlcude the line 2*/ 

    //For the number of matches that will be in this column
    for (var i = 0; i < matches; i++) {
        var matchDiv = document.createElement("div");
        matchDiv.className = "match";
        generateMatch(matchDiv, isFinals);
        columnDiv.appendChild(matchDiv);
    }


}

/** Function will generate a complete match for the tournament bracket
 * 
 * @param {any} matchDiv The Div element to append the match values to
 * @param {Boolean} isFinals Boolean value determining if the match is the finals match
 */
function generateMatch(matchDiv, isFinals) {
    //Add the player slots 
    generatePlayerSlot(matchDiv, "match-top team");
    generatePlayerSlot(matchDiv, "match-bottom team");

    //Create all the line related div's
    var matchLinesDiv = document.createElement("div");
    matchLinesDiv.className = "match-lines";

    //Create the First line div and append to the parent div
    var lineOneDiv = document.createElement("div");
    lineOneDiv.className = "line one";
    matchLinesDiv.appendChild(lineOneDiv);

    //If we are not in the finals match
    if (!isFinals) {
        //Create the "line two" line in the html file
        var lineTwoDiv = document.createElement("div");
        lineTwoDiv.className = "line two";

        matchLinesDiv.appendChild(lineTwoDiv);
    }

    //Append the lines to the match lines div
    matchDiv.appendChild(matchLinesDiv);

    //Create the Alt Lines Div (Converged line)
    var matchLinesAltDiv = document.createElement("div");
    matchLinesAltDiv.className = "match-lines alt";
    //The Line Div iteslf
    var lineOneAltDiv = document.createElement("div");
    lineOneAltDiv.className = "line one";
    matchLinesAltDiv.appendChild(lineOneAltDiv);

    matchDiv.appendChild(matchLinesAltDiv);

}

/** Function that will generate the player slots for each match in the bracket
 * 
 * @param {any} matchDiv The match Div that the slot will be appended to
 * @param {any} slotName The name of the slot we are creating. Will be Top or Bottom Team
 */
function generatePlayerSlot(matchDiv, slotName) {
    var participantDiv = document.createElement("div");
    participantDiv.className = slotName;

    //The slot elements
    var playerSlotDiv = document.createElement("div");
    playerSlotDiv.className = "player-slot";
    //Add event listeners for the drag and drop
    playerSlotDiv.addEventListener('drop', drop);
    playerSlotDiv.addEventListener('dragover', allowDrop);

    //Add the slot div to the 
    participantDiv.appendChild(playerSlotDiv);

    //Add the player slot
    matchDiv.appendChild(participantDiv);
}
