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
