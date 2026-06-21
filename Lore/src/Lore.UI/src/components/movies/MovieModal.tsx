'use client'

import { useEffect } from 'react'
import { Dialog, DialogPanel, DialogTitle } from '@headlessui/react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { X } from 'lucide-react'
import { Spinner } from '@/components/ui/Spinner'
import { REWATCH_STATUS_LABELS } from '@/types/movie'
import type { Movie } from '@/types/movie'
import type { CreateMoviePayload } from '@/lib/api/movies'

const schema = z.object({
  title: z.string().min(1, 'Required').max(128, 'Max 128 chars'),
  releaseYear: z.number().int().min(1888, 'Invalid year').max(2100, 'Invalid year'),
  durationMinutes: z.number().int().min(1, 'Must be > 0'),
  reviewText: z.string().max(10000).optional().nullable(),
  score: z.number().min(0).max(10).optional().nullable(),
  rewatchStatus: z.number().int().min(0).max(2),
})

type FormValues = z.infer<typeof schema>

interface MovieModalProps {
  open: boolean
  movie?: Movie | null
  prefillUniverseId?: number | null
  isPending: boolean
  onClose: () => void
  onSubmit: (payload: CreateMoviePayload) => Promise<void>
}

export function MovieModal({ open, movie, prefillUniverseId, isPending, onClose, onSubmit }: MovieModalProps) {
  const { register, handleSubmit, reset, formState: { errors } } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      title: '',
      releaseYear: new Date().getFullYear(),
      durationMinutes: 90,
      reviewText: null,
      score: null,
      rewatchStatus: 0,
    },
  })

  useEffect(() => {
    if (open) {
      reset({
        title: movie?.title ?? '',
        releaseYear: movie?.releaseYear ?? new Date().getFullYear(),
        durationMinutes: movie?.durationMinutes ?? 90,
        reviewText: movie?.reviewText ?? null,
        score: movie?.score ?? null,
        rewatchStatus: movie?.rewatchStatus ?? 0,
      })
    }
  }, [open, movie, reset])

  const submit = handleSubmit(async (values) => {
    const universeId = movie?.universeId ?? prefillUniverseId ?? null
    await onSubmit({
      title: values.title,
      releaseYear: values.releaseYear,
      durationMinutes: values.durationMinutes,
      reviewText: values.reviewText ?? null,
      score: values.score ?? null,
      viewCount: movie?.viewCount ?? 1,
      rewatchStatus: values.rewatchStatus,
      universeId,
      listNo: movie?.listNo ?? 0,
    })
  })

  const lockedUniverse = !movie && prefillUniverseId != null

  return (
    <Dialog open={open} onClose={onClose} className="relative z-50">
      <div className="fixed inset-0 bg-black/30" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4 overflow-y-auto">
        <DialogPanel className="w-full max-w-lg rounded-2xl bg-white p-6 shadow-xl my-4">
          <div className="flex items-center justify-between mb-5">
            <DialogTitle className="text-base font-semibold text-gray-900">
              {movie ? 'Edit movie' : 'New movie'}
            </DialogTitle>
            <button onClick={onClose} className="p-1.5 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition-colors">
              <X className="w-4 h-4" />
            </button>
          </div>

          {lockedUniverse && (
            <div className="mb-4 px-3 py-2 rounded-xl bg-indigo-50 border border-indigo-100 text-xs text-indigo-600">
              Will be linked to this universe automatically
            </div>
          )}

          <form onSubmit={submit} className="flex flex-col gap-4">
            <Field label="Title" error={errors.title?.message}>
              <input {...register('title')} placeholder="e.g. The Dark Knight" className={input} />
            </Field>

            <div className="grid grid-cols-2 gap-4">
              <Field label="Release year" error={errors.releaseYear?.message}>
                <input type="number" {...register('releaseYear', { valueAsNumber: true })} className={input} />
              </Field>
              <Field label="Duration (min)" error={errors.durationMinutes?.message}>
                <input type="number" {...register('durationMinutes', { valueAsNumber: true })} className={input} />
              </Field>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <Field label="Score (0–10)" error={errors.score?.message}>
                <input
                  type="number" step="0.1" min="0" max="10"
                  {...register('score', { setValueAs: v => v === '' ? null : parseFloat(v) })}
                  placeholder="—"
                  className={input}
                />
              </Field>
              <Field label="Rewatch status" error={errors.rewatchStatus?.message}>
                <select {...register('rewatchStatus', { valueAsNumber: true })} className={input}>
                  {([0, 1, 2] as const).map(s => (
                    <option key={s} value={s}>{REWATCH_STATUS_LABELS[s]}</option>
                  ))}
                </select>
              </Field>
            </div>

            <Field label="Review" error={errors.reviewText?.message}>
              <textarea
                {...register('reviewText')}
                rows={4}
                placeholder="Optional review…"
                className={`${input} resize-none`}
              />
            </Field>

            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={onClose} className="px-4 py-2 rounded-xl text-sm text-gray-500 hover:bg-gray-100 transition-colors">
                Cancel
              </button>
              <button
                type="submit"
                disabled={isPending}
                className="px-4 py-2 rounded-xl text-sm font-medium text-white bg-indigo-500 hover:bg-indigo-600 transition-colors disabled:opacity-50 flex items-center gap-2"
              >
                {isPending && <Spinner className="w-4 h-4 border-white border-t-transparent" />}
                Save
              </button>
            </div>
          </form>
        </DialogPanel>
      </div>
    </Dialog>
  )
}

const input = 'w-full rounded-xl border border-gray-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300'

function Field({ label, error, children }: { label: string; error?: string; children: React.ReactNode }) {
  return (
    <div className="flex flex-col gap-1">
      <label className="text-sm font-medium text-gray-700">{label}</label>
      {children}
      {error && <p className="text-xs text-rose-500">{error}</p>}
    </div>
  )
}
