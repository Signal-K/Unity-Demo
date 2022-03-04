// SPDX-Licenese-Identifier: MIT
pragma solidity ^0.8.0;

contract SubtractToken {
    // Demo function that subtracts a value when it's clicked/interacted with in Unity
    uint256 public tokenTotal = 0;

    function addTotal(uint8 _myArg) public {
        // Say if they were buying a token
        tokenTotal = tokenTotal + _myArg;
    }

    function subtractTotal(uint8 _myArg) public {
        // If end user is using a token
        tokenTotal = tokenTotal - _myArg;
    }
}
