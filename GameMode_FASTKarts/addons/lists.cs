//-----
// Misc. & Support Functions
//-----
//addItemToList, hasItemOnList and removeItemFromList by Ephialtes
//Useful functions for manipulating space delimited lists
function addItemToList(%string,%item)
{
	if(hasItemOnList(%string,%item))
		return %string;

	if(%string $= "")
		return %item;
	else
		return %string SPC %item;
}

function hasItemOnList(%string,%item)
{
	for(%i=0;%i<getWordCount(%string);%i++)
	{
		if(getWord(%string,%i) $= %item)
			return 1;
	}
	return 0;
}

function removeItemFromList(%string,%item)
{
	if(!hasItemOnList(%string,%item))
		return %string;

	for(%i=0;%i<getWordCount(%string);%i++)
	{
		if(getWord(%string,%i) $= %item)
		{
			if(%i $= 0)
				return getWords(%string,1,getWordCount(%string));
			else if(%i $= getWordCount(%string)-1)
				return getWords(%string,0,%i-1);
			else
				return getWords(%string,0,%i-1) SPC getWords(%string,%i+1,getWordCount(%string));
		}
	}
}