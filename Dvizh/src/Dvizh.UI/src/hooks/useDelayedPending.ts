'use client'

import { useEffect, useState } from 'react'
import { LOADING_DELAY_MS } from '@/lib/constants'

export function useDelayedPending(isPending: boolean): boolean {
  const [show, setShow] = useState(false)

  useEffect(() => {
    if (!isPending) {
      setShow(false)
      return
    }
    const timer = setTimeout(() => setShow(true), LOADING_DELAY_MS)
    return () => clearTimeout(timer)
  }, [isPending])

  return show
}
