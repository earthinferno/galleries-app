// stubbed data
const images = 
  [
    {
        id: 1,
        url: "./../../src/img/index.png",
        comment: "First Image",
        liked: false,
    },
    {
      id: 1,
      url: "./../../src/img/index.png",
      comment: "First Image",
      liked: false,
    },
    {
      id: 1,
      url: "./../../src/img/index.png",
      comment: "First Image",
      liked: false,
    }
  ]

export const FETCH_IMAGES = 'FETCH_IMAGES';

export const fetchImages = () => ({
  //return images;
    type: FETCH_IMAGES,
    images: images
  });