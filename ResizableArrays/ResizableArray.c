// --------------------------------------------------------------------------------
// Name: Matthew Collard
// Abstract: Resizable Arrays
// --------------------------------------------------------------------------------

// --------------------------------------------------------------------------------
// Includes – built-in libraries of functions
// --------------------------------------------------------------------------------
#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <math.h>

// --------------------------------------------------------------------------------
// Constants
// --------------------------------------------------------------------------------

// --------------------------------------------------------------------------------
// User Defined Types (UDT)
// --------------------------------------------------------------------------------

// --------------------------------------------------------------------------------
// Prototypes
// --------------------------------------------------------------------------------
void MakeArray( int** ppaintValues, int* pintArraySize );
void InitializeArray( int* paintValues, int intArraySize );
void PopulateArray( int* paintValues, int intArraySize );
void PrintArray( int* paintValues, int intArraySize );
int FindArrayMaximum( int* paintValues, int intArraySize );
int FindArrayMinimum( int* paintValues, int intArraySize );
int CalculateArrayTotal( int* paintValues, int intArraySize );
int CalculateArrayAverage( int* paintValues, int intArraySize );
float CalculateArrayStandardDeviation( int* paintValues, int intArraySize );
void AddValueToEndOfArray( int** ppaintValues, int* pintArraySize, int intValueToAdd );
void AddValueToFrontOfArray( int** ppaintValues, int* pintArraySize, int intValueToAdd );
void InsertValueIntoArray( int** ppaintValues, int* pintArraySize, int intValueToAdd, int intInsertIndex );
void RemoveValueFromArray( int** ppaintValues, int* pintArraySize, int intRemovalIndex );
void DeleteArray( int** ppaintValues, int* pintArraySize );


// --------------------------------------------------------------------------------
// Name: main
// Abstract: This is where the magic happens
// --------------------------------------------------------------------------------
void main( )
{
	int intIndex = 0;
	int intArraySize = 0;
	int* paintValues = 0;
	int intTotal = 0;
	int intResult = 0;
	int intAverage = 0;
	float sngStandardDeviation = 0;

	// Make & Initialize array (MakeArray calls InitializeArray)
	MakeArray( &paintValues, &intArraySize );

	// Populate array
	PopulateArray( paintValues, intArraySize );

	// Print array
	PrintArray( paintValues, intArraySize );

	// Array Maximum
	intResult = FindArrayMaximum( paintValues, intArraySize );
	printf( "Maximum array value = %d\n", intResult );
	printf( "\n" );

	// Array Minimum
	intResult = FindArrayMinimum( paintValues, intArraySize );
	printf( "Minimum array value = %d\n", intResult );
	printf( "\n" );

	// Calculate Array Total
	intTotal = CalculateArrayTotal( paintValues, intArraySize );
	printf( "Total = %d\n", intTotal );
	printf( "\n" );

	// Calculate Array Average
	intAverage = CalculateArrayAverage( paintValues, intArraySize );
	printf( "Average = %d\n", intAverage );
	printf( "\n" );

	// Calculate Array Standard Deviation
	sngStandardDeviation = CalculateArrayStandardDeviation( paintValues, intArraySize );
	printf( "Standard Deviation = %0.2f\n", sngStandardDeviation );
	printf( "\n" );

	// Add Value to End of Array
	AddValueToEndOfArray( &paintValues, &intArraySize, 10 );
	PrintArray( paintValues, intArraySize );

	// Add Value to Front of Array
	AddValueToFrontOfArray( &paintValues, &intArraySize, 10 );
	PrintArray( paintValues, intArraySize );

	// Insert Value in Middle of Arrya
	InsertValueIntoArray( &paintValues, &intArraySize, 5, 2 );
	PrintArray( paintValues, intArraySize );

	// Remove Value From Array
	RemoveValueFromArray( &paintValues, &intArraySize, 2 );
	PrintArray( paintValues, intArraySize );

	// Delete array
	DeleteArray( &paintValues, &intArraySize );

	system( "pause" );
}


#pragma region "Part 1"
// --------------------------------------------------------------------------------
// Name: MakeArray
// Abstract: Make the array based on user given size
// --------------------------------------------------------------------------------
void MakeArray( int** ppaintValues, int* pintArraySize )
{
	// How big?
	do
	{
		printf( "Enter array size:" );
		scanf( "%d", pintArraySize );
	} while ( *pintArraySize < 1 || *pintArraySize > 100000 );

	// Dynamically allocate memory
	*ppaintValues = (int*) malloc( sizeof(int) * *pintArraySize );

	InitializeArray( *ppaintValues, *pintArraySize );

	printf( "\n" );
}



// --------------------------------------------------------------------------------
// Name: InitializeArray
// Abstract: Initialize the array
// --------------------------------------------------------------------------------
void InitializeArray( int* paintValues, int intArraySize )
{
	int intIndex = 0;

	for( intIndex = 0; intIndex < intArraySize; intIndex += 1 )
	{
		// Base address + offset
		*( paintValues + intIndex ) = 0;
	}
}



// --------------------------------------------------------------------------------
// Name: PopulateArray
// Abstract: Populate the array variables
// --------------------------------------------------------------------------------
void PopulateArray( int* paintValues, int intArraySize )
{
	int intIndex = 0;

	printf( "Populate ----------\n" );
	for( intIndex = 0; intIndex < intArraySize; intIndex += 1 )
	{
		printf( "Enter value[ %d ]:", intIndex );

		// !!!!! No need for & because it already is an address/pointer]
		scanf( "%d", ( paintValues + intIndex ) );
	}

	printf( "\n" );
}



// --------------------------------------------------------------------------------
// Name: PrintArray
// Abstract: Print the array variables
// --------------------------------------------------------------------------------
void PrintArray( int* paintValues, int intArraySize )
{
	int intIndex = 0;

	printf( "Print ----------\n" );
	for( intIndex = 0; intIndex < intArraySize; intIndex += 1 )
	{
		printf( "Value[ %d ]: = %d\n", intIndex, *( paintValues + intIndex ) );
	}

	printf( "\n" );
}



// --------------------------------------------------------------------------------
// Name: FindArrayMaximum
// Abstract: Find the maximum array variable
// --------------------------------------------------------------------------------
int FindArrayMaximum( int* paintValues, int intArraySize )
{
	int intIndex = 0;
	int intMaximumValue = 0;

	// set max to first index in case of all negatives
	intMaximumValue = *( paintValues );

	for( intIndex = 0; intIndex < intArraySize; intIndex += 1 )
	{
		// Number at pointer is larger than current max value?
		if( *( paintValues + intIndex ) > intMaximumValue )
		{
			// Yes, set max value to larger number
			intMaximumValue = *( paintValues + intIndex );
		}
	}

	return intMaximumValue;
}



// --------------------------------------------------------------------------------
// Name: FindArrayMinimum
// Abstract: Find the minimum array variable
// --------------------------------------------------------------------------------
int FindArrayMinimum( int* paintValues, int intArraySize )
{
	int intIndex = 0;
	int intMinimumValue = 0;

	// set minimum to first index in case of negataives
	intMinimumValue = *( paintValues );

	for( intIndex = 0; intIndex < intArraySize; intIndex += 1 )
	{
		// Number at pointer is larger than current max value?
		if( *( paintValues + intIndex ) < intMinimumValue )
		{
			// Yes, set max value to larger number
			intMinimumValue = *( paintValues + intIndex );
		}
	}

	return intMinimumValue;
}



// --------------------------------------------------------------------------------
// Name: CalculateArrayTotal
// Abstract: Calculate the total of the array variables
// --------------------------------------------------------------------------------
int CalculateArrayTotal( int* paintValues, int intArraySize )
{
	int intIndex = 0;
	int intTotal = 0;

	for( intIndex = 0; intIndex < intArraySize; intIndex += 1 )
	{
		intTotal += *( paintValues + intIndex );
	}

	return intTotal;
}



// --------------------------------------------------------------------------------
// Name: CalculateArrayAverage
// Abstract: Calculate the average of the array variables
// --------------------------------------------------------------------------------
int CalculateArrayAverage( int* paintValues, int intArraySize )
{
	int intIndex = 0;
	int intTotal = 0;
	int intAverage = 0;

	intTotal = CalculateArrayTotal( paintValues, intArraySize );

	intAverage = intTotal / intArraySize;

	return intAverage;
}



// --------------------------------------------------------------------------------
// Name: CalculateArrayStandardDeviation
// Abstract: Calculate the standard deviation of array variables
// 
// standard deviation = square root of the variance
// variance = average of the squared differences from the MEAN
//
// --------------------------------------------------------------------------------
float CalculateArrayStandardDeviation( int* paintValues, int intArraySize )
{
	int intIndex = 0;
	int intAverage = 0;
	int intDifference = 0;
	float sngVariance = 0;
	float sngStandardDeviation = 0;

	intAverage = CalculateArrayAverage( paintValues, intArraySize );;

	for( intIndex = 0; intIndex < intArraySize; intIndex += 1 )
	{
		// How far from average?
		intDifference = *( paintValues + intIndex ) - intAverage;

		// square the difference
		sngVariance += (intDifference * intDifference);
	}

	// Average the result
	sngVariance /= intArraySize;

	// Standard Deviation = square root of variance
	sngStandardDeviation = sqrt(sngVariance);

	return sngStandardDeviation;
}
#pragma endregion

#pragma region "Part 2"
// --------------------------------------------------------------------------------
// Name: AddValueToEndOfArray
// Abstract: Add Value to end of the array
// --------------------------------------------------------------------------------
void AddValueToEndOfArray( int** ppaintValues, int* pintArraySize, int intValueToAdd )
{
	InsertValueIntoArray( ppaintValues, pintArraySize, 10, *pintArraySize );
}


#pragma region "Old Code 1"
// --------------------------------------------------------------------------------
// Name: AddValueToEndOfArray2
// Abstract: Add Value to end of the array
// --------------------------------------------------------------------------------
void AddValueToEndOfArray2( int** ppaintValues, int* pintArraySize, int intValueToAdd )
{
	int intNewSize = 0;
	int* paintNewValues = 0; 
	int intIndex = 0;

	// Allocate block of memory size + 1
	intNewSize = *pintArraySize + 1;
	paintNewValues = (int*) malloc( sizeof(int) * intNewSize );

	// Copy all the values from the old array into the new array
	for( intIndex = 0; intIndex < *pintArraySize; intIndex += 1 )
	{
		*( paintNewValues + intIndex ) = *( *ppaintValues + intIndex );
	}

	// Set the last array element value
	*( paintNewValues + intIndex ) = intValueToAdd;

	// Delete old array
	free( *ppaintValues );
	*ppaintValues = 0;

	// Assign pointer to new array to variable passed in 
	*ppaintValues = paintNewValues;
	*pintArraySize += 1;
}
#pragma endregion


// --------------------------------------------------------------------------------
// Name: AddValueToFrontOfArray
// Abstract: Add Value to the front of the array
// --------------------------------------------------------------------------------
void AddValueToFrontOfArray( int** ppaintValues, int* pintArraySize, int intValueToAdd )
{
	InsertValueIntoArray( ppaintValues, pintArraySize, 10, 0 );
}


#pragma region "Old Code 2"
// --------------------------------------------------------------------------------
// Name: AddValueToFrontOfArray2
// Abstract: Add Value to the front of the array
// --------------------------------------------------------------------------------
void AddValueToFrontOfArray2( int** ppaintValues, int* pintArraySize, int intValueToAdd )
{
	int intNewSize = 0;
	int* paintNewValues = 0; 
	int intIndex = 0;

	// Allocate block of memory size + 1
	intNewSize = *pintArraySize + 1;
	paintNewValues = (int*) malloc( sizeof(int) * intNewSize );

	// Copy all the values from the old array into the new array
	for( intIndex = 0; intIndex < *pintArraySize; intIndex += 1 )
	{
		*( paintNewValues + intIndex + 1 ) = *( *ppaintValues + intIndex );
	}

	// Set the last array element value
	*( paintNewValues + 0 ) = intValueToAdd;

	// Delete old array
	free( *ppaintValues );
	*ppaintValues = 0;

	// Assign pointer to new array to variable passed in 
	*ppaintValues = paintNewValues;
	*pintArraySize += 1;
}
#pragma endregion


// --------------------------------------------------------------------------------
// Name: InsertValueIntoArray
// Abstract: Insert value at a specific index
// --------------------------------------------------------------------------------
void InsertValueIntoArray( int** ppaintValues, int* pintArraySize, int intValueToAdd, int intInsertIndex )
{
	int intNewSize = 0;
	int* paintNewValues = 0; 
	int intIndex = 0;

	// Allocate block of memory size + 1
	intNewSize = *pintArraySize + 1;
	paintNewValues = (int*) malloc( sizeof(int) * intNewSize );

	// Boundary Check
	if( intInsertIndex > *pintArraySize ) intInsertIndex = *pintArraySize;
	if( intInsertIndex <              0 ) intInsertIndex = 0;

	// First loop pre insert
	for( intIndex = 0; intIndex < intInsertIndex; intIndex += 1 )
	{
		*( paintNewValues + intIndex ) = *( *ppaintValues + intIndex );
	}

	// Assign value
	*( paintNewValues + intIndex ) = intValueToAdd;

	// Second loop post insert
	for( intIndex = intInsertIndex + 1; intIndex < intNewSize; intIndex += 1)
	{
		*( paintNewValues + intIndex ) = *( *ppaintValues + intIndex - 1 );
	}

	// Delete old array
	free( *ppaintValues );
	*ppaintValues = 0;

	// Assign pointer to new array to variable passed in 
	*ppaintValues = paintNewValues;
	*pintArraySize += 1;
}



// --------------------------------------------------------------------------------
// Name: RemoveValueFromArray
// Abstract: Remove a value from a specific index
// --------------------------------------------------------------------------------
void RemoveValueFromArray( int** ppaintValues, int* pintArraySize, int intRemovalIndex )
{
	int intNewSize = 0;
	int* paintNewValues = 0; 
	int intIndex = 0;

	// Allocate block of memory size + 1
	intNewSize = *pintArraySize - 1;
	paintNewValues = (int*) malloc( sizeof(int) * intNewSize );

	// Is there anything in the array? 
	if( *pintArraySize > 0 )
	{
		// Boundary check
		if( intRemovalIndex > *pintArraySize - 1 ) intRemovalIndex = *pintArraySize - 1;
		if( intRemovalIndex <			       0 ) intRemovalIndex = 0;

		// First Loop Pre-Removal
		for( intIndex = 0; intIndex < intRemovalIndex; intIndex += 1 )
		{
			*( paintNewValues + intIndex ) = *( *ppaintValues + intIndex );
		}


		// Second Loop Post-Removal (skip ahead one index)
		for( intIndex = intRemovalIndex; intIndex < intNewSize; intIndex += 1)
		{
			*( paintNewValues + intIndex ) = *( *ppaintValues + intIndex + 1 );
		}

		// Delete old array
		free( *ppaintValues );
		*ppaintValues = 0;

		// Assign pointer to new array to variable passed in 
		*ppaintValues = paintNewValues;
		*pintArraySize -= 1;
	}
}
#pragma endregion


// --------------------------------------------------------------------------------
// Name: DeleteArray
// Abstract: Free up the memory allocated to the array
// --------------------------------------------------------------------------------
void DeleteArray( int** ppaintValues, int* pintArraySize )
{
	// Delete array
	free( *ppaintValues );
	*ppaintValues = 0;

	*pintArraySize = 0;
}
