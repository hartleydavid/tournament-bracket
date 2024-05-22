function allowDrop(event) {
    event.preventDefault();
}

function drag(event) {
    event.dataTransfer.setData("text", event.target.id);
}

function drop(event) {
    event.preventDefault();
    var data = event.dataTransfer.getData("text");
    event.target.appendChild(document.getElementById(data));
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

/**
 * 
 */
function generateTournamentBracket(participants, elementID) {
    //Get the tournament DIV
    var tournamentDiv = document.getElementById(elementID);

    //Create the bracket div
    var bracketDiv = document.createElement("div");
    bracketDiv.className = "bracket";

    //Switch case to send the number of columns to the respective dynamic function based on the number of participants
    switch (findSmallestMultiple(participants)) {
        case 4:
            multipleOfFour(Math.sqrt(participants), participants, bracketDiv);
            break;
        case 6:
            //multiple of 6
            break;
        default:
            //Prime number?
    }

    //Append the bracket to the html file
    tournamentDiv.appendChild(bracketDiv);
}

/**
 * 
 * @param {any} columns
 * @param {any} participants
 * @param {any} bracketDiv
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

/**
 * 
 * @param {any} bracketdiv
 */
function generateWinnerColumn(bracketdiv) {
    //Create the final column for the winner
    var winnerColumnDiv = document.createElement("div");
    winnerColumnDiv.className = "column last";

    var winnerDiv = document.createElement("div");
    winnerDiv.className = "winner";

    //Generate the winner slot "match"
    generatePlayerSlot(winnerDiv, "winner-slot");

    //Create line div's for CSS
    var matchLinesAltDiv = document.createElement("div");
    matchLinesAltDiv.className = "match-lines alt";

    var lineExtensionDiv = document.createElement("div");
    lineExtensionDiv.className = "line extended";

    //Append Elements 
    matchLinesAltDiv.appendChild(lineExtensionDiv);

    winnerColumnDiv.appendChild(winnerDiv);
    winnerColumnDiv.appendChild(matchLinesAltDiv);

    //append to the bracket
    bracketdiv.appendChild(winnerColumnDiv);
}

/**
 * 
 * @param {any} matches
 * @param {any} columnDiv
 * @param {any} isFinals
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

/**
 * 
 * @param {any} matchDiv
 * @param {any} isFinals
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

/**
 * 
 * @param {any} matchDiv
 * @param {any} isTop
 */
function generatePlayerSlot(matchDiv, slotName) {
    var participantDiv = document.createElement("div");
    participantDiv.className = slotName;

    //The slot elements
    var playerSlotDiv = document.createElement("div");
    playerSlotDiv.className = "player-slot";
    playerSlotDiv.addEventListener('drop', drop);
    playerSlotDiv.addEventListener('dragover', allowDrop);
    //playerSlotDiv.ondrop = "drop(event)";
    //playerSlotDiv.ondragover = "allowDrop(event)";

    participantDiv.appendChild(playerSlotDiv);

    //Add the player slot
    matchDiv.appendChild(participantDiv);
}
