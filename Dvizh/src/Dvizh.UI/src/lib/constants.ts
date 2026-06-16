export const LOADING_DELAY_MS = 500
export const DEFAULT_PAGE_SIZE = 20

if (!process.env.NEXT_PUBLIC_API_URL) {
  throw new Error('NEXT_PUBLIC_API_URL is not set')
}

export const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL

const CAT_IMAGE_BASE = `${API_BASE_URL}/img/mochi-peach-cat`

export const CAT_IMAGES = {
  neutral: `${CAT_IMAGE_BASE}/neutral.gif`,
  hoverYes: `${CAT_IMAGE_BASE}/hover-yes.gif`,
  hoverNo: `${CAT_IMAGE_BASE}/hover-no.gif`,
  yes: `${CAT_IMAGE_BASE}/yes.gif`,
  no: `${CAT_IMAGE_BASE}/no.gif`,
} as const

const DUCK_IMAGE_BASE = `${API_BASE_URL}/img/utya-duck`

export const DUCK_IMAGES = {
  neutral: `${DUCK_IMAGE_BASE}/neutral.gif`,
  hoverYes: `${DUCK_IMAGE_BASE}/hover-yes.gif`,
  hoverNo: `${DUCK_IMAGE_BASE}/hover-no.gif`,
  yes: `${DUCK_IMAGE_BASE}/yes.gif`,
  no: `${DUCK_IMAGE_BASE}/no.gif`,
} as const

export type MascotImages = typeof CAT_IMAGES
