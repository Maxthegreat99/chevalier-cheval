/*
 *==========*==========*==========*==========+=========*===========*==========*==========+
 **                                                                                     **
 **      copyright:  Copyright 2022, Fireball_2000, All rights reserved.                **
 **                                                                                     **
 **       Filename:  main.c                                                             **
 **                                                                                     **
 **    Description:  a VERY small game written in SDL                                   **
 **                                                                                     **
 **        Version:  1.0                                                                **
 **        Created:  01/02/23 23:31:07                                                  **
 **       Revision:  none                                                               **
 **       Compiler:  gcc                                                                **
 **                                                                                     **
 **         Author:  Fireball_2000 (FB), kerlan.jacquette@gmail.com                     **
 **        Company:  Trian Games                                                        **
 **                                                                                     **
 **       Language:  C                                                                  **
 **       Run time:                                                                     **
 **                                                                                     **
 *=======================================================================================+
 */

 /*=======================<START OF PROGRAM>=======================*/

/* HEADER FILE INCLUDES AND MACRO DEFINITIONS   ######################################## */
#include <math.h>
#include <SDL_events.h>
#include <SDL_rect.h>
#include <SDL_shape.h>
#include <SDL_surface.h>
#include <SDL_video.h>
#include <SDL_image.h>
#include <stdio.h>
#include <sys/time.h>
#include <SDL.h>

#define SCREEN_WIDTH 960
#define SCREEN_HEIGHT 540
/*  STRUCI DEFINITION   ################################################################# */
typedef struct Object
{
  SDL_Texture* spriteSheet;
  char* spriteSheetPath;
  SDL_Rect spriteRectOnSheet;
  SDL_Rect spriteRectDisplay;
  float y;
  float x;

}Object;
  /* initialize vars */
SDL_Event eventHandler;
float counter = 0.0;

SDL_Window* window = NULL;
SDL_Renderer* renderer = NULL;

SDL_Texture* background = NULL;



/*  FUNCTION DECLARATION   ############################################################## */

/*
 * ===  FUNCTION  ======================================================================
 *         Name:  INIT
 *  Description:  initializes SDL and the window plus creates the window
 * =====================================================================================
 */
  int
init (SDL_Window** window,SDL_Renderer** renderer)
{
  /* initialize sdl */
  int wasSDLinitSuccessful = SDL_Init(SDL_INIT_EVERYTHING );

  if( wasSDLinitSuccessful < 0){
    printf( "SDL could not initialize! SDL_Error: %s\n", SDL_GetError() );
    return 0;
  }
  /* enable linear texture filtering */
  if( !SDL_SetHint( SDL_HINT_RENDER_SCALE_QUALITY, "1" ) )
		printf( "Warning: Linear texture filtering not enabled!" );


  /* create the window */
  *window = SDL_CreateWindow( "SDL Tutorial", SDL_WINDOWPOS_UNDEFINED,
                                SDL_WINDOWPOS_UNDEFINED, SCREEN_WIDTH,
                                SCREEN_HEIGHT, SDL_WINDOW_SHOWN );
  if( *window == NULL){
    printf( "Window could not be created! SDL_Error: %s\n", SDL_GetError() );
    return 0;
  }
  /* create renderer */
  *renderer = SDL_CreateRenderer(*window,-1, SDL_RENDERER_PRESENTVSYNC | SDL_RENDERER_ACCELERATED);

  if( *renderer == NULL ){
    printf("couldnt create renderer SDL Error: %s\n", SDL_GetError() );
    return 0;
  }
  /* initialize renderer color */
  SDL_SetRenderDrawColor(*renderer, 0xFF, 0xFF, 0xFF, 0xFF );

  /* initialize png loading */
  int IMG_Flags = IMG_INIT_PNG;
  if ( ! ( IMG_Init(IMG_Flags) & IMG_Flags ) ){
    printf("SDL IMAGE COULD NOT BE INITIALIZED SDL ERROR: %s\n", IMG_GetError() );
    return 0;
  }
  return 1;
}		/* -----  end of function init  ----- */

/*
 * ===  FUNCTION  ======================================================================
 *         Name:  PLAYANIMATION
 *  Description:  plays the animation on a given sprite sheet
 * =====================================================================================
*/
  void
playAnimation(SDL_Texture* spriteSheet,int amountOfSprites,
              int positionToRenderX, int positionToRenderY,
              float *counter,SDL_Rect rectToDisplay)
{
  //get the height and width of sprite sheet
  SDL_Point sheetInfo;

  /* getting info for the texture */
  SDL_QueryTexture(spriteSheet, NULL, NULL, &sheetInfo.x, &sheetInfo.y);


  int widthOfOneImage = sheetInfo.x/amountOfSprites;

  int heightOfOneImage = sheetInfo.y;

  SDL_Rect rectToRender;

  /* getting the rect to take from the texture */
  int positionToRender = floor(*counter);
  printf("%d",positionToRenderX);

  rectToRender.h = heightOfOneImage;
  rectToRender.w = widthOfOneImage;
  rectToRender.y = 0;
  rectToRender.x = widthOfOneImage * positionToRender;

  rectToDisplay.x = positionToRenderX;
  rectToDisplay.y = positionToRenderY;
  SDL_RenderCopy(renderer,spriteSheet,&rectToRender,&rectToDisplay);

}
/*
 * ===  FUNCTION  ======================================================================
 *         Name:  LOAD MEDIA
 *  Description:  loads the images
 * =====================================================================================
 */
  int
loadMedia(SDL_Texture** arrayOfTextures[],int amountOfTextures,SDL_Renderer** Renderer
          ,char* imagePaths[])
{
  for(int i = 0;i < amountOfTextures;i++){
    /* load assets */
  	*arrayOfTextures[i] = IMG_LoadTexture(*Renderer,imagePaths[i]);
    if( *arrayOfTextures[i] == NULL){
      printf( "Failed to load image: %s\n",imagePaths[i] );
      return 0;
      }
  }
  return 1;
}		/* -----  end of function loadMedia  ----- */


/*
 * ===  FUNCTION  ======================================================================
 *         Name:  loadNinitialize
 *  Description:  initialize sdl subsystems and load media
 * =====================================================================================
 */
  int
loadNinitialize(SDL_Texture** arrayOfTextures[],int amountOfTextures,SDL_Renderer** Renderer
                ,char* imagePaths[],SDL_Window** window){
  /*initialize sdl2 */
  if(!init(window, Renderer)){
    printf( "Failed to initialize!\n" );
    return 0;
  }
  /* Load the media */
  if(!loadMedia(arrayOfTextures,amountOfTextures,Renderer,imagePaths)){
    printf("Failed to load media!\n");
    return 0;
  }

  return 1;
}    /*  -----  end of function loadNinitialize  ----- */

/*
 * ===  FUNCTION  ======================================================================
 *         Name:  handleEvents
 *  Description:  function for handling key presses for movement, returns
 *                a point for the power of each direction of the key press
 *                it also checks weathwe the program shoul exit
 * =====================================================================================
 */
  int
handleEvent(SDL_Event* e,Object* objectToMove,float power){

   SDL_PollEvent(e);

  /* get keyboard stats */
  if( e->type == SDL_KEYDOWN )
    {
        const Uint8* keystates = SDL_GetKeyboardState(NULL);

        //Adjust the velocity
        switch( e->key.keysym.sym )
        {

            case SDLK_LEFT:
              while(keystates[SDL_SCANCODE_LEFT] ){
                 objectToMove->x -= power;

                 SDL_RenderClear(renderer);
                 SDL_RenderCopy(renderer,background,NULL,NULL);

                 counter+=0.05;
                 playAnimation(objectToMove->spriteSheet,4,(int)objectToMove->x,(int)objectToMove->y,&counter,
                               objectToMove->spriteRectDisplay);

                 if(counter >= 4.05){
                    counter = 0.0;
                 }
                 static float counter2 = 0.0;
                 counter2 += 0.1;
                 if(counter2 >= 20.00){
                        printf("ASUS");
                   SDL_PumpEvents();
                 }
              }
              break;

            case SDLK_RIGHT:
              while(keystates[SDL_SCANCODE_RIGHT] ){
                 objectToMove->x += power;

                     SDL_RenderClear(renderer);
                     SDL_RenderCopy(renderer,background,NULL,NULL);

                     counter+=0.05;
                 playAnimation(objectToMove->spriteSheet,4,(int)objectToMove->x,(int)objectToMove->y,&counter,
                               objectToMove->spriteRectDisplay);

                if(counter >= 4.05){
                    counter = 0.0;
                }
                 static float counter2 = 0.0;
                 counter2 += 0.1;
                 if(counter2 >= 20.00){
                   SDL_PumpEvents();
                 }

              }
              break;
        }
    }
  return 1;
}  /*  -----  end of function  handleMovementEvent ----- */


/*
 * ===  FUNCTION  ======================================================================
 *         Name:  initObjectRect
 *  Description:  function used for  moving an object to a specified location
 * =====================================================================================
 */
  void
initObjectRect(Object* object,int spriteSheetColumns,int spriteSheetRows, int posX, int posY)
{
  SDL_Point objectInfo;
  SDL_QueryTexture(object->spriteSheet, NULL, NULL, &objectInfo.x, &objectInfo.y);

  object->spriteRectDisplay.h = objectInfo.y/spriteSheetRows;
  object->spriteRectDisplay.w = objectInfo.x/spriteSheetColumns;
  object->spriteRectDisplay.x = posX;
  object->spriteRectDisplay.y = posY;


}   /*  -----  end of function enterMainLoop  ----- */

/* * ===  FUNCTION  ======================================================================
 *         Name:  CLOSE
 *  Description:  quits sdl and free global surfaces and windows
 * =====================================================================================
 */
  void
end (SDL_Texture** textures,int amountOfTextures,SDL_Window** window,
     SDL_Renderer** Renderer)
{
  for(int i=0;i < amountOfTextures;i++)
  {
    SDL_DestroyTexture(textures[i]);
    textures[i] = NULL;
  }
  SDL_DestroyWindow(*window);
  *window =NULL;
  SDL_DestroyRenderer(*Renderer);
  *Renderer = NULL;
  /* quit SDL */
  SDL_Quit();
  IMG_Quit();
}		/* -----  end of function close  ----- */




/*
 * ===  FUNCTION  ======================================================================
 *         Name:  printHeading
 *  Description:  Prints the heading of the program before any
 *                significant operation is run.
 * =====================================================================================
 */

	void
printHeading(){
	printf("                       PLATFORM GAME\n");
printf("              BY FIREBALL_2000 OF TRIAN GAMES\n");
}    /*  -----  end of function printheading  ----- */

/* MAIN   ######################################################### */

	int
main( int argc, char* args[] ) {

  /* run time                   */
  struct timeval  tv1, tv2;
	gettimeofday(&tv1, NULL);

	/* MAIN FUNCTION CALLS   ######################################## */

  printHeading();
    /* initialize vars */
  int jumpVar = 0;
  int jumpPower = 50;
  Object character;
  character.spriteSheet = NULL;
  character.spriteSheetPath = "spritesheet.png";
  character.spriteRectOnSheet.w = 0;

  int isCharacterOnSolid = 1;
  character.x  =300.0;
  character.y = 200.0;

  /* initialize SDL2 */
  init(&window,&renderer);


  /* load media */
  SDL_Texture** texturesToInitialize[2] = {&character.spriteSheet, &background};
  char* texturePaths[2] = {character.spriteSheetPath ,"sus.png"};

  loadMedia(texturesToInitialize,2,&renderer,texturePaths);
  /* initialize character sprite */
  initObjectRect(&character,4,1,300,200);

  /* used to calculate fps */
  int newTime = 0;
  int lastFpsUpdate = SDL_GetTicks();
  /* print fps */
  printf("FPS: ");
  /*main loop starts */
  while(1){
    int oldTime = newTime;
    newTime = SDL_GetTicks();

    float fps = 1.f / ( (float)(newTime - oldTime) / 1000.f );
    /* print fps */

    if(newTime - lastFpsUpdate > 1000){
      printf("\rFPS: %3.2f    ",fps);
      lastFpsUpdate = SDL_GetTicks();
    }

    /* movement code */
    SDL_Event* e = &eventHandler;
    Object* objectToMove = &character;
    float power = 15;




       SDL_PollEvent(e);

  /* get keyboard stats */

        const Uint8* keystates = SDL_GetKeyboardState(NULL);

        //Adjust the velocity


             if(keystates[SDL_SCANCODE_LEFT] ){
                 objectToMove->x -= power;
                 static float counter2 = 0.0;
                 counter2 += 0.1;
                 if(counter2 >= 20.00){
                   printf("ASUS");
                   SDL_PumpEvents();
                 }

             }



              if(keystates[SDL_SCANCODE_RIGHT] ){
                 objectToMove->x += power;

                 static float counter2 = 0.0;
                 counter2 += 0.1;
                 if(counter2 >= 20.00){
                   SDL_PumpEvents();
                 }

              }

              if(keystates[SDL_SCANCODE_SPACE] ){
                jumpVar = 1;
                 static float counter2 = 0.0;
                 counter2 += 0.1;

                 if(counter2 >= 20.00){
                   SDL_PumpEvents();
                 }

              }

    static int counter5 = 0;
    counter5++;

    
    printf("%f",character.y);
    static int counterJump = 0;
    if(jumpVar > 0 && counterJump <= 100 && isCharacterOnSolid == 1 ){
      character.y -= jumpPower;
      counterJump++;
    }
    if(counterJump > (jumpPower/3)){
      jumpVar = 0;
      isCharacterOnSolid = 0;
    }
    static int counterFall = 0;
    if(isCharacterOnSolid == 0 && counterJump > (jumpPower/3) ){
      
      character.y += jumpPower;
      counterFall++;
    }
    if(counterFall != 0 && counterFall == counterJump){
      counterJump = 0;
      counterFall = 0;
      isCharacterOnSolid = 1;
    }
    /* clear screen and render background */
    SDL_RenderClear(renderer);
    SDL_RenderCopy(renderer,background,NULL,NULL);
    /* counter for animation playing */

    counter+=0.05;
    printf("%d\n",(int)character.x);
    playAnimation(character.spriteSheet,4,(int)character.x,(int)character.y,&counter,
                  character.spriteRectDisplay);
        printf("a");

    printf("a\n");
    if(counter >= 4.05)
      counter = 0.0;

    /*updateScreen*/
    SDL_RenderPresent(renderer);
    printf("a");

  }
  end(*texturesToInitialize,2,&window,&renderer);

  /*-------------------------------------------------------------- */

  /* print the run time          */

  gettimeofday(&tv2, NULL);
  double timeSpent = (double) (tv2.tv_usec - tv1.tv_usec) / 1000000 +
                     (double) (tv2.tv_sec - tv1.tv_sec);
  printf("\n= = = = = = = = = = = = = = = = = \n");
  printf("PROGRAM RUN TIME ===> %6.2fs\n",timeSpent);

  return 0;
}   /*  --------------  end of main  -------------- */


 /*========================<END OF PROGRAM>========================*/
