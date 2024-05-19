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
function generateTournamentBracket(participants) {
    //Get the tournament DIV
    var tournamentDiv = document.getElementById("dynamic-bracket");

    //Create the bracket div
    var bracketDiv = document.createElement("div");
    bracketDiv.className = "bracket";

    //Switch case to send the number of columns to the respective dynamic function based on the number of participants
    switch (findSmallestMultiple(participants)) {
        case 4:
            multipleOfFour(Math.sqrt(count), participants, bracketDiv);
            break;
        case 6:
            //multiple of 6
            break;
        default:
            //Prime number?
    }

    //Append the bracket to the html file
    tournamentDiv.appendChild(bracket);
}

function multipleOfFour(columns, participants, bracketDiv) {

    //The first round will have half of the number of participants of rounds
    var matchesPerColumn = participants / 2;

    //For each of the columns needed
    for (var i = 1; i <= columns; i++) {
        //generate the column
        var columnDiv = document.createElement("div");
        columnDiv.className = `column ${i}`; 

        generateColumn(matchesPerColumn, columnDiv);

        //Half the number of matches for the next round
        matchesPerColumn /= 2;
        //Append the column to the bracket
        bracketDiv.appendChild(columnDiv);
    }

}


function generateColumn(matches, columnDiv) {

    //For the number of matches that will be in this column
    for (var i = 0; i < matches; i++) {
        var matchDiv = document.createElement("div");
        matchDiv.className = "match";
        generateMatch(match);
        columnDiv.appendChild(matchDiv);
    }
}

function generateMatch(matchDiv) {
    //Add the player slots 
    generatePlayerSlot(matchDiv, true);
    generatePlayerSlot(matchDiv, false);

    //Create all the line related div's
    var matchLinesDiv = document.createElement("div");
    matchLinesDiv.className = "match-lines";

    var matchLinesAltDiv = document.createElement("div");
    matchLinesAltDiv.className = "match-lines alt";

    var lineOneDiv = document.createElement("div");
    lineOneDiv.className = "line one";

    var lineTwoDiv = document.createElement("div");
    lineTwoDiv.className = "line two";

    //Build the structure
    matchLinesDiv.appendChild(lineOneDiv);
    matchLinesDiv.appendChild(lineTwoDiv);
    matchDiv.appendChild(matchLinesDiv);
    matchLinesAltDiv.appendChild(lineOneDiv);

    matchDiv.appendChild(matchLinesAltDiv);

}

function generatePlayerSlot(matchDiv, isTop) {
    var participantDiv = document.createElement("div");
    participantDiv.className = isTop ? "match-top team" : "match-bottom team";

    //The slot elements
    var img = document.createElement("img");
    img.src = "https://tournamentbracketimages.blob.core.windows.net/publishedtesting-test/coolduck_400x400.jpg"
    img.alt = "playerSlot"

    //Add the player slot
    matchDiv.appendChild(participantDiv);
}


/**
            <div class="match">
                <div class="match-top team">
                    <img src="https://tournamentbracketimages.blob.core.windows.net/publishedtesting-test/coolduck_400x400.jpg" alt="Icon" />
                    <span class="name">Orlando </span>
                </div>
                <div class="match-bottom team">
                    <img src="https://tournamentbracketimages.blob.core.windows.net/publishedtesting-test/coolduck_400x400.jpg" alt="Icon" />
                    <span class="name">D.C. </span>
                </div>
                <div class="match-lines">
                    <div class="line one"></div>
                    <div class="line two"></div>
                </div>
                <div class="match-lines alt">
                    <div class="line one"></div>
                </div>
            </div>
 */
//tournament
    //bracket
        //Column
            //Match (no need for winner-top/bottom?)
                //Top Team
                //Bottom Team
                //Match Lines
                    //Line one
                    //Line two
                //Match Lines alt
                    //Line one


/**
 * Test Case 16 teams
 * Column 1: 16 Teams
 * Column 2: 8 Teams
 * Column 3: 4 Teams
 * Column 4: 2 Teams
 * Column 5: 1 Team
 * 
 * There are a sqrt() + 1 columns for 16
 * 
 */
