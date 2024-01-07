Zoo Animal Management System
Description: You are assigned to develop an Animal Transfer System in .NET
that facilitates the movement of animals to an empty zoo. The system should
consider specific rules and guidelines to ensure the appropriate grouping of
animals during the transfer process.
Features:
Implement a mechanism for zookeepers to accommodate animals from the input
JSON to the new zoo.
o The system should take into account the list of animals provided
and the number of new enclosures available in the new zoo. All
animals should fit.
o Animals should be transferred while adhering to the following rules:
▪ Vegetarian animals can be placed together in the same
enclosure.
▪ Animals of the same species should not be separated and
should be assigned to the same enclosure.
▪ Meat-eating animals of different species should preferably not
be grouped together in the same enclosure. However, if
necessary due to limited enclosures, only two different species
of meat-eating animals can be grouped together.
▪ Consider any additional constraints or guidelines you deem
necessary for the animal transfer process.
Input:
JSON (animals.json) full of animals with their specifications (food,
species...)
JSON (enclosures.json) full of enclosures with their specifications
(inside/outside, size, objects...)
Output:
Database filled with all the animals from the JSON. Each animal should
have an enclosure assigned to him.
