//import fetchImages from './server-actions-stub'

let nextImageId = 0;
export const addImage = (url,comment,liked) => ({
    type: 'ADD_IMAGE',
    id: nextImageId++,
    url,
    comment,
    liked,
});