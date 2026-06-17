'use client'

import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useRouter } from 'next/navigation'
import { Spinner } from '@/components/ui/Spinner'
import type { Universe } from '@/types/universe'
import type { CreateUniversePayload, UpdateUniversePayload } from '@/lib/api/universes'

const schema = z.object({
  name: z.string().min(1, 'Name is required').max(128, 'Max 128 characters'),
  description: z.string().max(2000).optional(),
  isHidden: z.boolean(),
  listNo: z.number().int().min(0),
})

type FormValues = z.infer<typeof schema>

interface UniverseFormProps {
  universe?: Universe
  onSubmit: (payload: CreateUniversePayload | UpdateUniversePayload) => Promise<void>
  isPending: boolean
}

export function UniverseForm({ universe, onSubmit, isPending }: UniverseFormProps) {
  const router = useRouter()

  const { register, handleSubmit, formState: { errors } } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      name: universe?.name ?? '',
      description: universe?.description ?? '',
      isHidden: universe?.isHidden ?? false,
      listNo: universe?.listNo ?? 0,
    },
  })

  const submit = handleSubmit(async (values) => {
    const payload = universe
      ? { id: universe.id, ...values, description: values.description || undefined }
      : { ...values, description: values.description || undefined }
    await onSubmit(payload)
    router.push('/universes')
  })

  return (
    <form onSubmit={submit} className="flex flex-col gap-5">
      <Field label="Name" error={errors.name?.message}>
        <input
          {...register('name')}
          placeholder="e.g. Harry Potter"
          className="w-full rounded-xl border border-gray-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
        />
      </Field>

      <Field label="Description" error={errors.description?.message}>
        <textarea
          {...register('description')}
          rows={4}
          placeholder="Optional description…"
          className="w-full rounded-xl border border-gray-200 px-3 py-2 text-sm resize-none focus:outline-none focus:ring-2 focus:ring-indigo-300"
        />
      </Field>

      <Field label="List order" error={errors.listNo?.message}>
        <input
          type="number"
          {...register('listNo', { valueAsNumber: true })}
          className="w-32 rounded-xl border border-gray-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
        />
      </Field>

      <label className="flex items-center gap-3 cursor-pointer">
        <input
          type="checkbox"
          {...register('isHidden')}
          className="w-4 h-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-300"
        />
        <span className="text-sm text-gray-700">Hidden from public</span>
      </label>

      <div className="flex gap-3 justify-end pt-2">
        <button
          type="button"
          onClick={() => router.push('/universes')}
          className="px-5 py-2 rounded-xl text-sm text-gray-500 hover:bg-gray-100 transition-colors"
        >
          Cancel
        </button>
        <button
          type="submit"
          disabled={isPending}
          className="px-5 py-2 rounded-xl text-sm font-medium text-white bg-indigo-500 hover:bg-indigo-600 transition-colors disabled:opacity-50 flex items-center gap-2"
        >
          {isPending && <Spinner className="w-4 h-4 border-white border-t-transparent" />}
          Save
        </button>
      </div>
    </form>
  )
}

function Field({ label, error, children }: { label: string; error?: string; children: React.ReactNode }) {
  return (
    <div className="flex flex-col gap-1">
      <label className="text-sm font-medium text-gray-700">{label}</label>
      {children}
      {error && <p className="text-xs text-rose-500">{error}</p>}
    </div>
  )
}
